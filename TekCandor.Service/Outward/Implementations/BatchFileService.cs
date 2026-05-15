using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class BatchFileService : IBatchFileService
    {
        private readonly IBatchFileRepository _batchFileRepository;

        public BatchFileService(IBatchFileRepository batchFileRepository)
        {
            _batchFileRepository = batchFileRepository;
        }

        public async Task<FileUploadResultDTO> ProcessUploadedFileAsync(IFormFile file, string uploadedBy)
        {
            try
            {


                string content;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    content = await reader.ReadToEndAsync();
                }

                var lines = content
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .ToList();


                var headerParts = lines[0].Split('|');
                if (headerParts.Length < 3)
                    throw new Exception("Invalid file: Header format incorrect");

                var branchName = headerParts[0].Trim();
                var totalInstruments = int.Parse(headerParts[1].Trim());
                var totalAmount = decimal.Parse(headerParts[2].Trim());

                var branch = await _batchFileRepository.GetBranchByNameAsync(branchName);
                if (branch == null)
                    throw new Exception($"Branch not found: {branchName}");

                var hasUploadedToday = await _batchFileRepository.HasBranchUploadedTodayAsync(branch.Code);
                if (hasUploadedToday)
                    throw new Exception($"Branch {branchName} has already uploaded a file today. Only one file upload per day is allowed.");

                var batchId = GenerateBatchId();


                var chequeLines = lines.Skip(1).ToList();
                var cheques = new List<ChequeInfo>();

                foreach (var line in chequeLines)
                {
                    var parts = line.Split('|');
                    if (parts.Length < 6) continue;

                    DateTime? chequeDate = null;
                    if (DateTime.TryParse(parts[5].Trim(), out DateTime parsedDate))
                    {
                        chequeDate = parsedDate;
                    }

                    cheques.Add(new ChequeInfo
                    {
                        BatchId = batchId,
                        Date = chequeDate,
                        AccountNo = parts[0].Trim(),
                        DepositorTitle = parts[1].Trim(),
                        ChequeNo = parts[2].Trim(),
                        BeneficiaryTitle = parts[3].Trim(),
                        Amount = decimal.Parse(parts[4].Trim()),
                        BranchName = branchName,
                        ReceiverBranchCode = branch.Code,
                        Status = "Pending",
                        CreatedOn = DateTime.Now,
                        CreatedBy = uploadedBy
                    });
                }

                if (cheques.Count != totalInstruments)
                    throw new Exception($"File validation failed: Expected {totalInstruments} instruments but found {cheques.Count}");

                var calculatedAmount = cheques.Sum(c => c.Amount ?? 0);
                if (calculatedAmount != totalAmount)
                    throw new Exception($"File validation failed: Expected amount {totalAmount} but calculated {calculatedAmount}");

                var batch = new Batch
                {
                    BatchId = batchId,
                    Branch = branch.Code,
                    TotalInstruments = totalInstruments,
                    TotalAmount = totalAmount,
                    Status = "Draft",
                    CreatedAt = DateTime.Now,
                    CreatedBy = uploadedBy
                };

                await _batchFileRepository.SaveBatchAsync(batch);

                await _batchFileRepository.SaveChequesAsync(cheques);

                return new FileUploadResultDTO
                {
                    BatchId = batchId,
                    BranchName = branchName,
                    BranchCode = branch.Code,
                    TotalInstruments = totalInstruments,
                    TotalAmount = totalAmount,
                    Status = "Draft",
                    Cheques = cheques.Select(c => new ChequeInfoDTO
                    {
                        AccountNo = c.AccountNo,
                        DepositorTitle = c.DepositorTitle,
                        ChequeNo = c.ChequeNo,
                        BeneficiaryTitle = c.BeneficiaryTitle,
                        Amount = c.Amount
                    }).ToList()
                };

            }
            catch (Exception ex)
            {
            }
            return new FileUploadResultDTO
            {
                BatchId = null,
                BranchName = null,
                BranchCode = null,
                TotalInstruments = 0,
                TotalAmount = 0,
                Status = "Error",
                Cheques = new List<ChequeInfoDTO>()
            };
        }

        private string GenerateBatchId()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random().Next(1, 99);
            return $"BATCH-{timestamp}-{random}";
        }
    }
}
