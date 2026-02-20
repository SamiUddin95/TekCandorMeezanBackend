using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class ChequeDepositImportService : IChequeDepositImportService
    {
        private readonly IImportDataRepository _importDataRepository;
        private readonly IManualImportDataRepository _manualImportDataRepository;
        private readonly IChequeDepositRepository _chequeDepositRepository;
        private readonly ILogger<ChequeDepositImportService> _logger;

        public ChequeDepositImportService(
            IImportDataRepository importDataRepository,
            IManualImportDataRepository manualImportDataRepository,
            IChequeDepositRepository chequeDepositRepository,
            ILogger<ChequeDepositImportService> logger)
        {
            _importDataRepository = importDataRepository;
            _manualImportDataRepository = manualImportDataRepository;
            _chequeDepositRepository = chequeDepositRepository;
            _logger = logger;
        }

        public async Task<bool> ImportSFTPFilesAsync(string hostName, int port, string username, string password, string remoteLocation, string localLocation)
        {
            try
            {
                using var client = new SftpClient(hostName, port, username, password);
                _logger.LogInformation($"Connecting to SFTP server: {hostName}");
                
                client.Connect();
                var files = client.ListDirectory(remoteLocation);
                
                foreach (var file in files.Where(f => !f.IsDirectory))
                {
                    var fileName = file.Name.Replace(".txt", "");
                    
                    // Check for duplicate files
                    var importDataExists = await _importDataRepository.GetByFileNameAsync(fileName);
                    var manualImportExists = await _manualImportDataRepository.GetByFileNameAsync(fileName);
                    
                    if (importDataExists != null || manualImportExists != null)
                    {
                        _logger.LogWarning($"File already uploaded: {file.Name}");
                        continue;
                    }

                    string remoteFilePath = Path.Combine(remoteLocation, file.Name);
                    string localFilePath = Path.Combine(localLocation, file.Name);
                    
                    _logger.LogInformation($"Downloading file: {remoteFilePath} to {localFilePath}");
                    
                    using (var fileStream = File.OpenWrite(localFilePath))
                    {
                        client.DownloadFile(remoteFilePath, fileStream);
                    }
                    
                    // Delete remote file after successful download
                    client.DeleteFile(remoteFilePath);
                }
                
                client.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during SFTP file import");
                throw;
            }
        }

        public async Task<List<string>> ProcessImportedFilesAsync(string folderPath, string callbackLimit)
        {
            var skippedFiles = new List<string>();
            try
            {
                const int batchSize = 1000; // Process records in batches
                var files = Directory.GetFiles(folderPath);
                var random = new Random();

                if (files.Length == 0)
                {
                    skippedFiles = ["File not Found"];
                    return skippedFiles;
                }

                foreach (var filePath in files)
                {
                    var fileInfo = new FileInfo(filePath);
                    var fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);

                    var importDataExists = await _importDataRepository.GetByFileNameAsync(fileName);
                    var manualImportExists = await _manualImportDataRepository.GetByFileNameAsync(fileName);

                    if (importDataExists != null || manualImportExists != null)
                    {
                        _logger.LogWarning($"File already uploaded: {fileName}");
                        skippedFiles.Add($"File already uploaded: {fileInfo.Name}");
                        continue;
                    }

                    var lines = await File.ReadAllLinesAsync(filePath);
                    
                    long importDataId = 0;
                    int successCount = 0;
                    int failureCount = 0;

                    var errorRecords = new List<ImportDataDetail>();
                    var chequeDeposits = new List<ChequedepositInfo>();
                    var currentBatch = 0;

                    foreach (var line in lines)
                    {
                        var fields = line.Split('|');
                        
                        if (fields.Length == 3)
                        {
                            // Header record
                            var totalRecords = Convert.ToInt64(fields[1]);
                            var importData = new ImportData
                            {
                                FileName = fileName,
                                Date = DateTime.Today,
                                TotalRecords = Convert.ToInt32(totalRecords),
                                SuccessfullRecords = 0,
                                FailureRecords = 0
                            };
                            
                            importDataId = await _importDataRepository.AddAsync(importData);
                            continue;
                        }

                        if (fields.Length == 41)
                        {
                            try
                            {
                                var chequeDeposit = await ParseChequeDepositDataAsync(fields, random, callbackLimit);
                                chequeDeposits.Add(chequeDeposit);
                                currentBatch++;

                                if (currentBatch >= batchSize)
                                {
                                    // Bulk insert cheque deposits
                                    await _chequeDepositRepository.AddRangeAsync(chequeDeposits);
                                    successCount += chequeDeposits.Count;
                                    chequeDeposits.Clear();
                                    currentBatch = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error processing line: {Line}", line);
                                errorRecords.Add(new ImportDataDetail
                                {
                                    ImportDataId = importDataId,
                                    Data = line,
                                    Date = DateTime.Now,
                                    Error = true,
                                    ErrorDescription = ex.Message,
                                    IsDeleted = false,
                                    CreatedOn = DateTime.Now
                                });

                                if (errorRecords.Count >= batchSize)
                                {
                                    // Bulk insert error records
                                    await _importDataRepository.AddDetailRangeAsync(errorRecords);
                                    failureCount += errorRecords.Count;
                                    errorRecords.Clear();
                                }
                            }
                        }
                    }

                    // Update import statistics
                    await _importDataRepository.UpdateStatisticsAsync(importDataId, successCount, failureCount);

                    // Move processed file
                    var processedPath = Path.Combine(folderPath, "Processed", fileInfo.Name);
                    Directory.CreateDirectory(Path.GetDirectoryName(processedPath));
                    File.Move(filePath, processedPath, true);
                }

                return skippedFiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing imported files");
                throw;
            }
        }

        private async Task<ChequedepositInfo> ParseChequeDepositDataAsync(string[] fields, Random random, string callbackLimit)
        {
            var dateParts = fields[0].Split('-');
            var dateStr = $"{dateParts[0]}-{dateParts[1]}-{dateParts[2]}";
            
            var chequeDeposit = new ChequedepositInfo
            {
                Date = DateTime.Parse(dateStr),
                CycleCode = fields[1],
                CityCode = fields[2],
                serialNo = fields[3],
                SenderBankCode = fields[4],
                SenderBranchCode = fields[5],
                ReceiverBankCode = fields[6],
                ReceiverBranchCode = fields[7],
                ChequeNumber = fields[8],
                AccountNumber = fields[9],
                SequenceNumber = fields[10],
                TransactionCode = fields[11],
                InstrumentNo = fields[11],
                Amount = decimal.Parse(fields[12]),
                // ... [Additional field mappings]
                stan = random.Next(0, 1000000).ToString("D6"),
                HubCode = await GetHubCodeAsync(fields[7]),
                Export = false,
                Callback = decimal.Parse(fields[12]) > decimal.Parse(callbackLimit),
                status = "P",
                Importtime = DateTime.Today,
                Error = false
            };

            // Process error fields and set error flags
            ProcessErrorFields(chequeDeposit, fields);

            return chequeDeposit;
        }

        private void ProcessErrorFields(ChequedepositInfo chequeDeposit, string[] fields)
        {
            var errorFields = new[]
            {
                ("IQATag", 13),
                ("DocumentSkew", 14),
                ("Piggyback", 15),
                ("ImageToolight",16),
                ("HorizontalStreaks", 17),
                ("BelowMinimumCompressedImageSize", 18),
                ("AboveMaximumCompressedImageSize", 19),
                ("UndersizeImage",20),
                ("FoldedorTornDocumentCorners", 21),
                ("FoldedOrTornDocumentEdges", 22),
                ("FramingError", 23),
                ("OversizeImage",24),
                ("ImageTooDark", 25),
                ("FrontRearDimensionMismatch", 26),
                ("CarbonStrip", 27),
                ("OutOfFocus",28),
                ("QRCodeMatch", 29),
                ("UV",30),
                ("MICRPresent", 31),
                ("StandardCheque", 32),
                ("InstrumentDuplicate", 33),
                ("AverageChequeAmount",34),
                ("TotalChequesCount",35),
                ("TotalChequesReturnCount", 36),
                ("CaptureAtNIFT_Branch", 37),
                ("DeferredCheque", 38),
                ("FraudChequeHistory",39),
            };

            foreach (var (fieldName, index) in errorFields)
            {
                if (fields[index] == "1")
                {
                    chequeDeposit.Error = true;
                    chequeDeposit.ErrorInFields += $"{fieldName} is not Ok,{Environment.NewLine}";
                }
            }
        }

        public async Task<string> GetHubCodeAsync(string receiverBranchCode)
        {
            return await _chequeDepositRepository.GetHubCodeAsync(receiverBranchCode);
        }

        public async Task<List<string>> ProcessManualImportAsync(string folderPath, string callbackLimit, string processedFolderPath)
        {
            var skippedFiles = new List<string>();
            try
            {
                const int batchSize = 1000;
                var files = Directory.GetFiles(folderPath);
                var random = new Random();

                if (files.Length == 0)
                {
                    skippedFiles = ["File not Found"];
                    return skippedFiles;
                }

                foreach (var filePath in files)
                {
                    var fileInfo = new FileInfo(filePath);
                    var fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);

                    // Check for duplicate files
                    var importDataExists = await _importDataRepository.GetByFileNameAsync(fileName);
                    var manualImportExists = await _manualImportDataRepository.GetByFileNameAsync(fileName);

                    if (importDataExists != null || manualImportExists != null)
                    {
                        _logger.LogWarning($"File already uploaded: {fileName}");
                        skippedFiles.Add($"File already uploaded: {fileInfo.Name}");
                        continue;
                    }

                    var lines = await File.ReadAllLinesAsync(filePath);
                    var successCount = 0;
                    var failureCount = 0;
                    long importDataId = 0;

                    var errorRecords = new List<Manual_ImportDataDetails>();
                    var chequeDeposits = new List<ChequedepositInfo>();
                    var currentBatch = 0;

                    foreach (var line in lines)
                    {
                        var fields = line.Split('|');

                        if (fields.Length == 3)
                        {
                            var totalRecords = Convert.ToInt64(fields[1]);
                            var importData = new Manual_ImportData
                            {
                                FileName = fileName,
                                Date = DateTime.Today,
                                TotalRecords = Convert.ToInt32(totalRecords),
                                SuccessfullRecords = 0,
                                FailureRecords = 0,
                                IsDeleted = false
                            };

                            importDataId = await _manualImportDataRepository.AddAsync(importData);
                            continue;
                        }

                        if (fields.Length == 41)
                        {
                            try
                            {
                                var chequeDeposit = await ParseChequeDepositDataAsync(fields, random, callbackLimit);
                                chequeDeposits.Add(chequeDeposit);
                                currentBatch++;

                                if (currentBatch >= batchSize)
                                {
                                    await _chequeDepositRepository.AddRangeAsync(chequeDeposits);
                                    successCount += chequeDeposits.Count;
                                    chequeDeposits.Clear();
                                    currentBatch = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error processing line: {Line}", line);
                                errorRecords.Add(new Manual_ImportDataDetails
                                {
                                    Data = line,
                                    Date = DateTime.Now,
                                    Error = true,
                                    ErrorDescription = ex.Message,
                                    Manual_ImportDataId = importDataId,
                                    IsDeleted = false,
                                    CreatedOn = DateTime.Now
                                });

                                if (errorRecords.Count >= batchSize)
                                {
                                    await _manualImportDataRepository.AddDetailRangeAsync(errorRecords);
                                    failureCount += errorRecords.Count;
                                    errorRecords.Clear();
                                }
                            }
                        }
                    }

                    // Process any remaining records
                    if (chequeDeposits.Any())
                    {
                        await _chequeDepositRepository.AddRangeAsync(chequeDeposits);
                        successCount += chequeDeposits.Count;
                    }

                    if (errorRecords.Any())
                    {
                        await _manualImportDataRepository.AddDetailRangeAsync(errorRecords);
                        failureCount += errorRecords.Count;
                    }

                    // Update statistics
                    await _manualImportDataRepository.UpdateStatisticsAsync(importDataId, successCount, failureCount);

                    // Move processed file
                    var processedPath = Path.Combine(processedFolderPath, fileInfo.Name);
                    Directory.CreateDirectory(Path.GetDirectoryName(processedPath));

                    if (File.Exists(processedPath))
                    {
                        File.Delete(filePath);
                    }
                    else
                    {
                        File.Move(filePath, processedPath, true);
                    }
                }

                return skippedFiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing manual import files");
                throw;
            }
        }
    }
}
