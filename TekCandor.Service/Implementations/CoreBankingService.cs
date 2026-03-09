using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Models;
using TekCandor.Service.Interfaces;

namespace TekCandor.Service.Implementations
{
    public class CoreBankingService : ICoreBankingService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoreBankingService> _logger;

        public CoreBankingService(
            AppDbContext context,
            IConfiguration configuration,
            ILogger<CoreBankingService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task ImportRecordsAfterImportFileAsync()
        {
            await ImportRecordsAfterImportFileAsync(false);
        }

        public async Task ImportRecordsAfterImportFileAsync(bool sendSms)
        {
            try
            {
                var today = DateTime.Today;
                var tomorrow = DateTime.Today.AddDays(1);

                // Step 1: Update PO accounts and special accounts to mark serviceRun = 1
                await UpdatePOAccountsServiceRunAsync(today);

                // Step 2: Get distinct account numbers that need account info retrieval
                var accountsToProcess = await GetAccountsToProcessAsync(today, tomorrow);

                if (accountsToProcess.Count > 0)
                {
                    _logger.LogInformation($"Processing {accountsToProcess.Count} accounts for account information retrieval");

                    // Step 3: Process accounts in parallel to get account information
                    await ProcessAccountInformationAsync(accountsToProcess, today);
                }

                // Step 4: Send callback emails for high-value transactions
                await SendCallbackEmailsAsync(today, tomorrow);

                // Step 5: Send SMS alerts if requested
                if (sendSms)
                {
                    await SendChequeReceiveSMSAlertsAsync();
                }

                _logger.LogInformation("ImportRecordsAfterImportFile completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImportRecordsAfterImportFile");
                throw;
            }
        }

        private async Task UpdatePOAccountsServiceRunAsync(DateTime today)
        {
            try
            {
                // Update PO accounts and special accounts
                var poAccounts = await _context.chequedepositInformation
                    .Where(x => x.Date == today &&
                               (x.AccountNumber.StartsWith("00017571") ||
                                x.AccountNumber.StartsWith("00017574") ||
                                x.AccountNumber == "0000000000000000" ||
                                x.AccountNumber.StartsWith("0000000")))
                    .ToListAsync();

                foreach (var account in poAccounts)
                {
                    account.serviceRun = true;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated {poAccounts.Count} PO/special accounts with serviceRun=1");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PO accounts service run");
                throw;
            }
        }

        private async Task<List<string>> GetAccountsToProcessAsync(DateTime today, DateTime tomorrow)
        {
            try
            {
                var accounts = await _context.chequedepositInformation
                    .Where(x => x.Date >= today &&
                               x.Date < tomorrow &&
                               x.status == "P" &&
                               x.serviceRun == false &&
                               !x.AccountNumber.StartsWith("00017571") &&
                               !x.AccountNumber.StartsWith("00017574") &&
                               x.AccountNumber != "0000000000000000")
                    .GroupBy(x => x.AccountNumber)
                    .Select(g => g.Key)
                    .ToListAsync();

                return accounts.Where(a => !string.IsNullOrEmpty(a)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accounts to process");
                throw;
            }
        }

        private async Task ProcessAccountInformationAsync(List<string> accounts, DateTime today)
        {
            try
            {
                var baseUrl = _configuration["CoreBanking:AccountInfoApiBaseUrl"];
                
                if (string.IsNullOrEmpty(baseUrl))
                {
                    _logger.LogWarning("CoreBanking:AccountInfoApiBaseUrl not configured. Skipping account info retrieval.");
                    return;
                }

                var userId = _configuration["CoreBanking:UserID"];
                var password = _configuration["CoreBanking:Password"];
                var channelType = _configuration["CoreBanking:AccInfoChannelType"];
                var channelSubType = _configuration["CoreBanking:AccInfoChannelSubType"];
                var transactionType = _configuration["CoreBanking:AccInfoTransactionType"];
                var transactionSubType = _configuration["CoreBanking:AccInfoTransactionSubType"];
                var function = _configuration["CoreBanking:AccInfoFunction"];
                var pan = _configuration["CoreBanking:AccInfoPAN"];
                var thumbprint = _configuration["CoreBanking:ThumbPrint"];

                // Get certificate
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                
                if (certificates.Count == 0)
                {
                    store.Close();
                    _logger.LogError("Certificate not found with thumbprint: {Thumbprint}", thumbprint);
                    return;
                }

                X509Certificate2 certificate = certificates[0];
                store.Close();

                // Process accounts in parallel
                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };
                var random = new Random();

                Parallel.ForEach(accounts, parallelOptions, (accountNumber) =>
                {
                    try
                    {
                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                        request.ClientCertificates.Add(certificate);
                        request.Method = "POST";
                        request.ContentType = "application/xml";

                        // Build SOAP request
                        XmlDocument soapReqBody = new XmlDocument();
                        string branchId = accountNumber.Length >= 6 ? accountNumber.Substring(2, 4) : "0000";
                        string accountId = accountNumber.Length > 6 ? accountNumber.Remove(0, 6) : accountNumber;
                        string stan = random.Next(0, 1000000).ToString("D6");

                        soapReqBody.LoadXml($@"<MBLCustAccInfo>
                            <UserID>{userId}</UserID>
                            <Password>{password}</Password>
                            <ChannelType>{channelType}</ChannelType>
                            <ChannelSubType>{channelSubType}</ChannelSubType>
                            <TransactionType>{transactionType}</TransactionType>
                            <TransactionSubType>{transactionSubType}</TransactionSubType>
                            <TranDateAndTime>{DateTime.Now:yyyy-MM-ddThh:mm:ss}</TranDateAndTime>
                            <Function>{function}</Function>
                            <HostData>
                                <MBLCustAccInfoReq>
                                    <STAN>{stan}</STAN>
                                    <PAN>{pan}</PAN>
                                    <BranchID>{branchId}</BranchID>
                                    <AccountID>{accountId}</AccountID>
                                </MBLCustAccInfoReq>
                            </HostData>
                        </MBLCustAccInfo>");

                        var reqDoc = XDocument.Parse(soapReqBody.InnerXml);
                        using (Stream stream = request.GetRequestStream())
                        {
                            reqDoc.Save(stream);
                        }

                        using (WebResponse serviceRes = request.GetResponse())
                        using (StreamReader rd = new StreamReader(serviceRes.GetResponseStream()))
                        {
                            var serviceResult = rd.ReadToEnd();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(serviceResult);

                            var finalRes = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented));

                            // Update cheque deposit information
                            UpdateChequeDepositFromApiResponse(accountNumber, today, finalRes);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing account info for account: {AccountNumber}", accountNumber);
                    }
                });

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing account information");
                throw;
            }
        }

        private void UpdateChequeDepositFromApiResponse(string accountNumber, DateTime today, dynamic apiResponse)
        {
            try
            {
                var hostData = apiResponse?.MBLCustAccInfoResponse?.hostData?.MBLCustAccInfoRsp;
                if (hostData == null) return;

                var cheques = _context.chequedepositInformation
                    .Where(x => x.AccountNumber == accountNumber && x.Date == today)
                    .ToList();

                foreach (var cheque in cheques)
                {
                    cheque.AccountTitle = hostData.CustomerName?.ToString()?.Replace("'", "") ?? cheque.AccountTitle;
                    cheque.MobileNo = hostData.Mobile?.ToString() ?? cheque.MobileNo;
                    cheque.Currency = hostData.Currency?.ToString() ?? cheque.Currency;
                    cheque.AccountBalance = hostData.WorkingBalance?.ToString() ?? cheque.AccountBalance;
                    cheque.OldAccount = hostData.IbanOfAccount?.ToString() ?? cheque.OldAccount;

                    // Set account status
                    if (hostData.Premium?.ToString() == "Y")
                        cheque.AccountStatus = "Premium";
                    else
                        cheque.AccountStatus = "Normal";

                    // Handle inactive marker
                    if (hostData.InactiveMarker?.ToString() == "Y")
                    {
                        cheque.status = "RE";
                        cheque.serviceRun = true;
                        cheque.PostRestriction = "Account Inactive";
                    }
                    else if (!string.IsNullOrEmpty(hostData.PostingRestriction?.ToString()))
                    {
                        string postingRestriction = hostData.PostingRestriction.ToString();
                        var allowedRestrictions = new[] { "21", "19", "02", "04", "08", "10", "28" };

                        if (Array.IndexOf(allowedRestrictions, postingRestriction) >= 0)
                        {
                            cheque.status = "P";
                            cheque.serviceRun = true;
                        }
                        else
                        {
                            // Materialize the query first to avoid dynamic operation in expression tree
                            var allRestrictions = _context.PostingRestriction.ToList();
                            var restriction = allRestrictions.FirstOrDefault(x => x.Code == postingRestriction);
                            
                            if (restriction != null)
                                cheque.PostRestriction = restriction.Code;
                            
                            cheque.status = "RE";
                            cheque.serviceRun = true;
                        }
                    }
                    else
                    {
                        cheque.serviceRun = true;
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cheque deposit from API response for account: {AccountNumber}", accountNumber);
            }
        }

        private async Task SendCallbackEmailsAsync(DateTime today, DateTime tomorrow)
        {
            try
            {
                var callbackAmountSetting = await _context.Setting
                    .Where(x => x.Name == "generalsettings.callbackamount")
                    .Select(x => x.Value)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(callbackAmountSetting) || !decimal.TryParse(callbackAmountSetting, out decimal callbackAmount))
                {
                    _logger.LogWarning("Callback amount setting not found or invalid. Skipping callback emails.");
                    return;
                }

                // Get accounts that need callback emails
                var accountsForEmail = await _context.chequedepositInformation
                    .Where(x => x.Date >= today &&
                               x.Date < tomorrow &&
                               x.status == "P" &&
                               x.Amount >= callbackAmount &&
                               x.CallbackEmailSend == false &&
                               !x.AccountNumber.StartsWith("00017571") &&
                               !x.AccountNumber.StartsWith("00017574") &&
                               x.AccountNumber != "0000000000000000")
                    .Join(_context.Branch,
                          chq => chq.AccountNumber.Substring(2, 4),
                          branch => branch.Code,
                          (chq, branch) => new
                          {
                              chq.AccountNumber,
                              ReceiverBranchCode = chq.AccountNumber.Substring(2, 4),
                              branch.Email1,
                              branch.Email2,
                              branch.Email3
                          })
                    .GroupBy(x => new { x.AccountNumber, x.ReceiverBranchCode, x.Email1, x.Email2, x.Email3 })
                    .Select(g => g.Key)
                    .ToListAsync();

                if (accountsForEmail.Count > 0)
                {
                    _logger.LogInformation($"Found {accountsForEmail.Count} accounts requiring callback emails");

                    // Get certificate for email service
                    var thumbprint = _configuration["CoreBanking:ThumbPrint"];
                    var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly);
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                    
                    if (certificates.Count == 0)
                    {
                        store.Close();
                        _logger.LogError("Certificate not found for email service");
                        return;
                    }

                    X509Certificate2 cert = certificates[0];
                    store.Close();

                    foreach (var account in accountsForEmail)
                    {
                        try
                        {
                            await SendEmailAlertAsync(account.ReceiverBranchCode, account.Email1, account.Email2, account.Email3, cert);
                            
                            // Update CallbackEmailSend flag
                            var chequesToUpdate = await _context.chequedepositInformation
                                .Where(x => x.AccountNumber.Substring(2, 4) == account.ReceiverBranchCode && 
                                           x.Date >= today && x.Date < tomorrow)
                                .ToListAsync();

                            foreach (var cheque in chequesToUpdate)
                            {
                                cheque.CallbackEmailSend = true;
                            }

                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Callback email sent successfully for branch {account.ReceiverBranchCode}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error sending email for branch {BranchCode}", account.ReceiverBranchCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending callback emails");
                // Don't throw - email failures shouldn't stop the entire process
            }
        }

        private async Task SendEmailAlertAsync(string branchCode, string emailTo, string emailCc, string emailBcc, X509Certificate2 certificate)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                var userId = _configuration["Email:UserID"];
                var password = _configuration["Email:Password"];
                var svcName = _configuration["Email:SvcName"];
                var channelType = _configuration["Email:ChannelType"];
                var channelSubType = _configuration["Email:ChannelSubType"];
                var accountNumber = _configuration["Email:AccountNumber"];
                var msgTemplateId = _configuration["Email:MsgTemplateId"];
                var currency = _configuration["Email:Currency"];
                var amount = _configuration["Email:Amount"];
                var branchName = _configuration["Email:BranchName"];
                var branchCodeConfig = _configuration["Email:BranchCode"];
                var chequeNo = _configuration["Email:ChequeNum"];
                var limit = _configuration["Email:Limit"];
                var emailSubject = _configuration["Email:Subject"];
                var emailText = _configuration["Email:Text"];

                // Build email alert request XML
                var emailRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<IFXEMAILALERTRequest>
    <SignonRq>
        <SignonPswd>
            <CustId>
                <CustLoginId>{userId}</CustLoginId>
            </CustId>
            <CustPswd>
                <CryptPswd>
                    <CustPassword>{password}</CustPassword>
                    <BinLength>0</BinLength>
                    <BinData>{password}</BinData>
                    <AuthenticationCode>{password}</AuthenticationCode>
                </CryptPswd>
            </CustPswd>
        </SignonPswd>
    </SignonRq>
    <BankSvcRq>
        <RqUID></RqUID>
        <SvcName>{svcName}</SvcName>
        <EMAILALERTRq>
            <ChannelType>{channelType}</ChannelType>
            <ChannelSubType>{channelSubType}</ChannelSubType>
            <TransactionType></TransactionType>
            <TransactionSubType></TransactionSubType>
            <AccountNo>{accountNumber}</AccountNo>
            <MsgTemplateId>{msgTemplateId}</MsgTemplateId>
            <CellNo></CellNo>
            <EmailTo>{emailTo}</EmailTo>
            <EmailCc>{emailCc}</EmailCc>
            <EmailBcc>{emailBcc}</EmailBcc>
            <Currency>{currency}</Currency>
            <Amount>{amount}</Amount>
            <WHT></WHT>
            <DrOrCr></DrOrCr>
            <TransactionDate>{DateTime.Today:dd-MM-yy}</TransactionDate>
            <TransactionTime></TransactionTime>
            <Narration></Narration>
            <Balance></Balance>
            <BranchName>{branchName}</BranchName>
            <BrachCode>{branchCodeConfig}</BrachCode>
            <CHEQUENO>{chequeNo}</CHEQUENO>
            <SRCBRANCHNAME></SRCBRANCHNAME>
            <SRCBRANCHCODE></SRCBRANCHCODE>
            <Depositor></Depositor>
            <Limit>{limit}</Limit>
            <EmailSubject>{emailSubject}</EmailSubject>
            <EmailText>{emailText}</EmailText>
            <IsQueuedOnly></IsQueuedOnly>
            <FileBase64String></FileBase64String>
            <FileBase64String1></FileBase64String1>
            <FileDirectory></FileDirectory>
            <FileName></FileName>
            <FileName1></FileName1>
        </EMAILALERTRq>
    </BankSvcRq>
</IFXEMAILALERTRequest>";

                var emailServiceUrl = _configuration["Email:ServiceUrl"];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(emailServiceUrl);
                request.ClientCertificates.Add(certificate);
                request.Method = "POST";
                request.ContentType = "application/xml";

                XmlDocument reqDoc = new XmlDocument();
                reqDoc.LoadXml(emailRequest);

                using (Stream stream = request.GetRequestStream())
                {
                    reqDoc.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    XmlDocument responseDoc = new XmlDocument();
                    responseDoc.LoadXml(responseText);

                    // Parse response to check status
                    var statusNode = responseDoc.SelectSingleNode("//Status/StatusCode");
                    if (statusNode?.InnerText != "00")
                    {
                        _logger.LogWarning("Email alert service returned non-success status: {Status}", statusNode?.InnerText);
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendEmailAlertAsync");
                throw;
            }
        }

        public async Task SendChequeReceiveSMSAlertsAsync()
        {
            try
            {
                var today = DateTime.Today;
                var tomorrow = DateTime.Today.AddDays(1);

                // Get cheques that need SMS alerts
                var chequesForSMS = await _context.chequedepositInformation
                    .Where(x => x.Date >= today &&
                               x.Date < tomorrow &&
                               x.IsDeleted == false &&
                               !x.AccountNumber.StartsWith("00017574") &&
                               !x.AccountNumber.StartsWith("00017571") &&
                               x.chqRecSMSSend == false)
                    .ToListAsync();

                if (chequesForSMS.Count > 0)
                {
                    _logger.LogInformation($"Sending SMS alerts for {chequesForSMS.Count} cheques");

                    foreach (var cheque in chequesForSMS)
                    {
                        try
                        {
                            await SendChequeReceiveSMSAlertAsync(
                                cheque.Id,
                                cheque.stan ?? string.Empty,
                                cheque.AccountNumber ?? string.Empty,
                                cheque.Amount?.ToString() ?? "0",
                                cheque.Currency ?? "PKR",
                                cheque.MobileNo ?? string.Empty,
                                cheque.ChequeNumber ?? string.Empty
                            );
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error sending SMS for cheque ID: {ChequeId}", cheque.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendChequeReceiveSMSAlertsAsync");
                throw;
            }
        }

        private async Task SendChequeReceiveSMSAlertAsync(long id, string stan, string accountNumber, string amount, string currency, string mobileNo, string chequeNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(mobileNo))
                {
                    _logger.LogWarning("Mobile number is empty for cheque ID: {ChequeId}", id);
                    return;
                }

                var baseUrl = _configuration["SMS:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    _logger.LogWarning("SMS:BaseUrl not configured. Skipping SMS alert.");
                    return;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                // Get certificate
                var thumbprint = _configuration["CoreBanking:ThumbPrint"];
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count == 0)
                {
                    store.Close();
                    _logger.LogError("Certificate not found for SMS service");
                    return;
                }

                X509Certificate2 certificate = certificates[0];
                store.Close();

                // Get configuration values
                var userId = _configuration["SMS:UserID"];
                var password = _configuration["SMS:Password"];
                var channelType = _configuration["SMS:ChannelType"];
                var channelSubType = _configuration["SMS:ChannelSubType"];
                var transactionType = _configuration["SMS:TransactionType"];
                var transactionSubType = _configuration["SMS:TransactionSubType"];
                var function = _configuration["SMS:Function"];
                var msgTemplateId = _configuration["SMS:RecMsgTemplateId"];
                var language = _configuration["SMS:TemplateLanguage"];
                var carrier = _configuration["SMS:TemplateCarrier"];

                // Mask account number for SMS (show last 4 digits)
                string maskedAccount = accountNumber.Length > 4 
                    ? "xxxxxxxxxx" + accountNumber.Substring(accountNumber.Length - 4) 
                    : accountNumber;

                string narration = $"Your cheque#{chequeNumber} of PKR {amount} drawn on a/c {maskedAccount} is received in inward clearing on {DateTime.Now:dd-MM-yy} at Meezan Bank. Visit your branch for details";

                // Build SMS alert request XML
                var smsRequest = $@"<SMSAlertApi>
                    <UserID>{userId}</UserID>
                    <Password>{password}</Password>
                    <ChannelType>{channelType}</ChannelType>
                    <ChannelSubType>{channelSubType}</ChannelSubType>
                    <TransactionType>{transactionType}</TransactionType>
                    <TransactionSubType>{transactionSubType}</TransactionSubType>
                    <TranDateAndTime>{DateTime.Now:yyyy-MM-ddThh:mm:ss}</TranDateAndTime>
                    <Function>{function}</Function>
                    <MsgTemplateId>{msgTemplateId}</MsgTemplateId>
                    <HostData>
                        <AlertReq>
                            <CellNo>{mobileNo}</CellNo>
                            <Language>{language}</Language>
                            <Narration>{narration}</Narration>
                            <Carrier>{carrier}</Carrier>
                        </AlertReq>
                    </HostData>
                </SMSAlertApi>";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                request.ClientCertificates.Add(certificate);
                request.Method = "POST";
                request.ContentType = "text/xml";

                XmlDocument reqDoc = new XmlDocument();
                reqDoc.LoadXml(smsRequest);

                using (Stream stream = request.GetRequestStream())
                {
                    reqDoc.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    XmlDocument responseDoc = new XmlDocument();
                    responseDoc.LoadXml(responseText);

                    // Log the request and response
                    _logger.LogInformation("SMS Alert sent for cheque ID: {ChequeId}, Account: {Account}", id, accountNumber);

                    // Parse response to check status
                    var statusNode = responseDoc.SelectSingleNode("//StatusCode");
                    if (statusNode?.InnerText == "00")
                    {
                        // Update the cheque record
                        var cheque = await _context.chequedepositInformation
                            .FirstOrDefaultAsync(x => x.Id == id);

                        if (cheque != null)
                        {
                            cheque.serviceRun = true;
                            cheque.chqRecSMSSend = true;
                            await _context.SaveChangesAsync();
                        }

                        _logger.LogInformation("SMS sent successfully for cheque ID: {ChequeId}", id);
                    }
                    else
                    {
                        _logger.LogWarning("SMS service returned non-success status: {Status} for cheque ID: {ChequeId}", 
                            statusNode?.InnerText, id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendChequeReceiveSMSAlertAsync for cheque ID: {ChequeId}", id);
                throw;
            }
        }

        public async Task GetSignaturesAsync()
        {
            try
            {
                var today = DateTime.Today;
                var tomorrow = DateTime.Today.AddDays(1);

                // Step 1: Update PO accounts and special accounts to mark isEditing = 1
                await UpdatePOAccountsSignatureAsync(today);

                // Step 2: Get distinct account numbers that need signature retrieval
                var accountsToProcess = await GetAccountsForSignatureAsync(today, tomorrow);

                if (accountsToProcess.Count > 0)
                {
                    _logger.LogInformation($"Processing {accountsToProcess.Count} accounts for signature retrieval");

                    // Step 3: Process accounts to get signatures
                    await ProcessSignatureRetrievalAsync(accountsToProcess);
                }

                _logger.LogInformation("GetSignatures completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSignatures");
                throw;
            }
        }

        private async Task UpdatePOAccountsSignatureAsync(DateTime today)
        {
            try
            {
                var poAccounts = await _context.chequedepositInformation
                    .Where(x => x.Date == today &&
                               (x.AccountNumber.StartsWith("00017571") ||
                                x.AccountNumber.StartsWith("00017574") ||
                                x.AccountNumber == "0000000000000000" ||
                                x.AccountNumber.StartsWith("000000")))
                    .ToListAsync();

                foreach (var account in poAccounts)
                {
                    account.isEditing = true;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated {poAccounts.Count} PO/special accounts with isEditing=1");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PO accounts for signature");
                throw;
            }
        }

        private async Task<List<string>> GetAccountsForSignatureAsync(DateTime today, DateTime tomorrow)
        {
            try
            {
                var accounts = await _context.chequedepositInformation
                    .Where(x => x.Date >= today &&
                               x.Date < tomorrow &&
                               x.status == "P" &&
                               x.isEditing == false &&
                               !x.AccountNumber.StartsWith("00017571") &&
                               !x.AccountNumber.StartsWith("00017574") &&
                               x.AccountNumber != "0000000000000000" &&
                               !x.AccountNumber.StartsWith("000000"))
                    .GroupBy(x => x.AccountNumber)
                    .Select(g => g.Key)
                    .ToListAsync();

                return accounts.Where(a => !string.IsNullOrEmpty(a)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accounts for signature");
                throw;
            }
        }

        private async Task ProcessSignatureRetrievalAsync(List<string> accounts)
        {
            try
            {
                var sigBaseUrl = _configuration["Signature:BaseUrl"];
                if (string.IsNullOrEmpty(sigBaseUrl))
                {
                    _logger.LogWarning("Signature:BaseUrl not configured. Skipping signature retrieval.");
                    return;
                }

                var userId = _configuration["CoreBanking:UserID"];
                var password = _configuration["CoreBanking:Password"];
                var channelType = _configuration["CoreBanking:ChannelType"];
                var channelSubType = _configuration["CoreBanking:ChannelSubType"];
                var transactionType = _configuration["CoreBanking:TransactionType"];
                var transactionSubType = _configuration["CoreBanking:TransactionSubType"];
                var sigFunction = _configuration["Signature:Function"];
                var thumbprint = _configuration["CoreBanking:ThumbPrint"];

                // Get certificate
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count == 0)
                {
                    store.Close();
                    _logger.LogError("Certificate not found for signature service");
                    return;
                }

                X509Certificate2 certificate = certificates[0];
                store.Close();

                foreach (var accountNumber in accounts)
                {
                    try
                    {
                        await ProcessSingleAccountSignatureAsync(accountNumber, sigBaseUrl, userId, password, 
                            channelType, channelSubType, transactionType, transactionSubType, sigFunction, certificate);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing signature for account: {AccountNumber}", accountNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessSignatureRetrievalAsync");
                throw;
            }
        }

        private async Task ProcessSingleAccountSignatureAsync(string accountNumber, string sigBaseUrl, 
            string userId, string password, string channelType, string channelSubType, 
            string transactionType, string transactionSubType, string sigFunction, X509Certificate2 certificate)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sigBaseUrl);
                request.ClientCertificates.Add(certificate);
                request.Method = "POST";
                request.ContentType = "application/xml";

                // Build signature request XML
                string criteriaValue = accountNumber.Length > 6 ? accountNumber.Remove(0, 6) : accountNumber;
                
                var signatureRequest = $@"<MBLT24Signature>
                    <UserID>{userId}</UserID>
                    <Password>{password}</Password>
                    <ChannelType>{channelType}</ChannelType>
                    <ChannelSubType>{channelSubType}</ChannelSubType>
                    <TransactionType>{transactionType}</TransactionType>
                    <TransactionSubType>{transactionSubType}</TransactionSubType>
                    <TranDateAndTime>{DateTime.Now:yyyy-MM-ddThh:mm:ss}</TranDateAndTime>
                    <Function>{sigFunction}</Function>
                    <HostData>
                        <MBLT24SignatureReq>
                            <criteriaValue>{criteriaValue}</criteriaValue>
                        </MBLT24SignatureReq>
                    </HostData>
                </MBLT24Signature>";

                XmlDocument reqDoc = new XmlDocument();
                reqDoc.LoadXml(signatureRequest);

                using (Stream stream = request.GetRequestStream())
                {
                    reqDoc.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    XmlDocument responseDoc = new XmlDocument();
                    responseDoc.LoadXml(responseText);

                    var jsonText = JsonConvert.SerializeXmlNode(responseDoc, Newtonsoft.Json.Formatting.Indented);
                    var responseObj = JObject.Parse(jsonText);

                    // Process signature response
                    await ProcessSignatureResponseAsync(accountNumber, responseObj);
                }

                // Update isEditing flag
                var cheques = await _context.chequedepositInformation
                    .Where(x => x.AccountNumber == accountNumber)
                    .ToListAsync();

                foreach (var cheque in cheques)
                {
                    cheque.isEditing = true;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessSingleAccountSignatureAsync for account: {AccountNumber}", accountNumber);
                throw;
            }
        }

        private async Task ProcessSignatureResponseAsync(string accountNumber, JObject response)
        {
            try
            {
                var signatureData = response.SelectToken("MBLT24SignatureResponse.hostData.Signature");
                
                if (signatureData == null)
                {
                    _logger.LogWarning("No signature data found for account: {AccountNumber}", accountNumber);
                    return;
                }

                var sftpHost = _configuration["Signature:SftpHost"];
                var sftpPort = int.Parse(_configuration["Signature:SftpPort"] ?? "22");
                var sftpUsername = _configuration["Signature:SftpUsername"];
                var sftpPassword = _configuration["Signature:SftpPassword"];
                var localPath = _configuration["Signature:LocalPath"];
                var sigImagesPath = _configuration["Signature:ImagesPath"];

                if (signatureData is JArray signatureArray)
                {
                    // Multiple signatures
                    await DownloadSignaturesAsync(signatureArray, accountNumber, sftpHost, sftpPort, 
                        sftpUsername, sftpPassword, localPath, sigImagesPath);
                }
                else if (signatureData is JObject singleSignature)
                {
                    // Single signature
                    var array = new JArray { singleSignature };
                    await DownloadSignaturesAsync(array, accountNumber, sftpHost, sftpPort, 
                        sftpUsername, sftpPassword, localPath, sigImagesPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing signature response for account: {AccountNumber}", accountNumber);
                throw;
            }
        }

        private async Task DownloadSignaturesAsync(JArray signatures, string accountNumber, 
            string sftpHost, int sftpPort, string sftpUsername, string sftpPassword, 
            string localPath, string sigImagesPath)
        {
            try
            {
                using (var sftpClient = new SftpClient(sftpHost, sftpPort, sftpUsername, sftpPassword))
                {
                    sftpClient.Connect();

                    foreach (var sig in signatures)
                    {
                        try
                        {
                            var pathToken = sig.SelectToken("Path");
                            if (pathToken == null) continue;

                            string remotePath = pathToken.ToString();
                            
                            // Clean up the path
                            remotePath = remotePath.Replace("/IBM/WebSphere/AppServer/profiles/AppSrv01/installedApps/was21Cell01/R16_TLIVE21_war.ear/R16_TLIVE21.war/im.images", "");

                            if (!sftpClient.Exists(remotePath))
                            {
                                _logger.LogWarning("Signature file not found: {Path}", remotePath);
                                continue;
                            }

                            if (string.IsNullOrEmpty(Path.GetExtension(remotePath)))
                            {
                                continue;
                            }

                            string fileName = Path.GetFileName(remotePath);

                            // Handle file extension normalization
                            if (fileName.EndsWith(".00") || fileName.EndsWith(".01"))
                            {
                                fileName = fileName.Substring(0, fileName.Length - 3) + ".jpg";
                            }
                            else if (fileName.EndsWith(".01.jpg"))
                            {
                                fileName = fileName.Substring(0, fileName.Length - 7) + ".jpg";
                            }

                            string localFilePath = Path.Combine(localPath, fileName);

                            // Download file
                            using (var fileStream = File.OpenWrite(localFilePath))
                            {
                                sftpClient.DownloadFile(remotePath, fileStream);
                            }

                            // Insert signature record
                            var signature = new TekCandor.Repository.Entities.Signature
                            {
                                AccountNumber = accountNumber,
                                Sign = System.Text.Encoding.UTF8.GetBytes(sigImagesPath + fileName),
                                CreatedOn = DateTime.Now,
                                IsDeleted = false
                            };

                            _context.Signatures.Add(signature);
                            await _context.SaveChangesAsync();

                            _logger.LogInformation("Downloaded signature: {FileName} for account: {AccountNumber}", 
                                fileName, accountNumber);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error downloading individual signature for account: {AccountNumber}", accountNumber);
                        }
                    }

                    sftpClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DownloadSignaturesAsync for account: {AccountNumber}", accountNumber);
                throw;
            }
        }

        public async Task<MBLChequePostingExt?> MBLPendingChequePostingAsync(long id, string stan, string accountNumber, string chequeNumber, string currency, string amount, string cycleCode, string hubCode, string approverId, string authorizerId)
        {
            try
            {
                var chequeData = await _context.chequedepositInformation.FindAsync(id);
                if (chequeData == null)
                {
                    _logger.LogError("Cheque deposit not found: {Id}", id);
                    return null;
                }

                var transTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                int maxRetryAttempts = int.Parse(_configuration["CoreBanking:MaxRetryAttempts"] ?? "3");
                int retryDelayMilliseconds = 1000;
                var baseUrl = _configuration["CoreBanking:ChequePostBaseUrl"];
                var thumbprint = _configuration["CoreBanking:ThumbPrint"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count == 0)
                {
                    store.Close();
                    _logger.LogError("Certificate not found");
                    return null;
                }

                X509Certificate2 certificate = certificates[0];
                store.Close();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                request.ClientCertificates.Add(certificate);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Timeout = 120000;

                // Get credit account based on cycle code
                var creditAccount = await GetCreditAccountAsync(cycleCode, hubCode);

                var amountDecimal = Convert.ToDecimal(amount);
                var userId = _configuration["CoreBanking:UserID"];
                var password = _configuration["CoreBanking:Password"];
                var channelType = _configuration["CoreBanking:ChequePostChannelType"];
                var channelSubType = _configuration["CoreBanking:ChequePostChannelSubType"];
                var transactionType = _configuration["CoreBanking:ChequePostTransactionType"];
                var transactionSubType = _configuration[$"CoreBanking:{cycleCode}"];
                var function = _configuration["CoreBanking:ChequePostFunction"];
                var pan = _configuration["CoreBanking:ChequePostPAN"];

                var debitBranch = accountNumber.Substring(2, 4);
                var debitAccount = accountNumber.Length > 6 ? accountNumber.Remove(0, 6) : accountNumber;

                var requestXml = $@"<MBLFT>
                    <UserID>{userId}</UserID>
                    <Password>{password}</Password>
                    <ChannelType>{channelType}</ChannelType>
                    <ChannelSubType>{channelSubType}</ChannelSubType>
                    <TransactionType>{transactionType}</TransactionType>
                    <TransactionSubType>{transactionSubType}</TransactionSubType>
                    <TranDateAndTime>{DateTime.Now:yyyy-MM-ddThh:mm:ss}</TranDateAndTime>
                    <Function>{function}</Function>
                    <HostData>
                        <FTRequest>
                            <STAN>{stan}</STAN>
                            <PAN>{pan}</PAN>
                            <DebitBranch>{debitBranch}</DebitBranch>
                            <DebitAccount>{debitAccount}</DebitAccount>
                            <DebitCurrency>{currency}</DebitCurrency>
                            <CreditBranch>{debitBranch}</CreditBranch>
                            <CreditAccount>{creditAccount}</CreditAccount>
                            <CreditCurrency>{currency}</CreditCurrency>
                            <Amount>{amountDecimal}</Amount>
                            <TxnNarration>{cycleCode}</TxnNarration>
                            <ChrgDebitBranch></ChrgDebitBranch>
                            <ChrgDebitAccount></ChrgDebitAccount>
                            <ChrgDebitCurrency></ChrgDebitCurrency>
                            <ChrgCreditBranch></ChrgCreditBranch>
                            <ChrgCreditAccount></ChrgCreditAccount>
                            <ChrgCreditCurrency></ChrgCreditCurrency>
                            <ChAmount></ChAmount>
                            <ChNarration></ChNarration>
                            <ForeignAmount></ForeignAmount>
                            <ExchangeRate></ExchangeRate>
                            <ForeignCCY></ForeignCCY>
                            <OperatorId></OperatorId>
                            <SupervisorId></SupervisorId>
                            <InstrType></InstrType>
                            <InstrNo>{chequeNumber}</InstrNo>
                            <InstrDate></InstrDate>
                            <Balance></Balance>
                            <Distribution></Distribution>
                            <OurIncomeAmt></OurIncomeAmt>
                            <OurIncomeCode></OurIncomeCode>
                            <FintechIncomeAmt></FintechIncomeAmt>
                            <FintechIncomeCode></FintechIncomeCode>
                            <BeneficiaryName></BeneficiaryName>
                            <BankName></BankName>
                            <Reserved2></Reserved2>
                            <Reserved3></Reserved3>
                            <Reserved4></Reserved4>
                        </FTRequest>
                    </HostData>
                </MBLFT>";

                _logger.LogInformation("Cheque Posting Request: {Request}", requestXml);

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentLength = bytes.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(bytes, 0, bytes.Length);
                }

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                var responseText = await reader.ReadToEndAsync();
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(responseText);

                                var jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                                _logger.LogInformation("Cheque Posting Response: {Response}", jsonText);

                                var finalRes = JsonConvert.DeserializeObject<MBLChequePostingExt>(jsonText);

                                if (finalRes?.MBLFT?.hostData?.hostFTResponse != null)
                                {
                                    if (finalRes.MBLFT.hostData.hostFTResponse.HostCode == "00")
                                    {
                                        chequeData.CoreFTId = finalRes.MBLFT.hostData.hostFTResponse.TransactionID;
                                        chequeData.PostingTime = transTime;
                                        chequeData.ApproverId = approverId;
                                        chequeData.AuthorizerId = authorizerId;
                                        chequeData.status = "A";
                                        chequeData.stan = stan;
                                        chequeData.TrProcORRecTime = DateTime.Now.ToString("yy-MM-dd:hh:mm:tt");

                                        int retryAttempt = 0;
                                        bool updateSuccess = false;

                                        while (!updateSuccess && retryAttempt < maxRetryAttempts)
                                        {
                                            try
                                            {
                                                await _context.SaveChangesAsync();
                                                updateSuccess = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.LogError(ex, "Retry {Attempt} for cheque {ChequeNumber}", retryAttempt + 1, chequeNumber);
                                                retryAttempt++;
                                                await Task.Delay(retryDelayMilliseconds);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        chequeData.status = "RE";
                                        chequeData.PostRestriction = finalRes.MBLFT.hostData.hostFTResponse.HostDesc;
                                        await _context.SaveChangesAsync();
                                    }

                                    return finalRes;
                                }
                            }
                        }
                    }
                }
                catch (WebException webEx)
                {
                    if (webEx.Status == WebExceptionStatus.Timeout)
                    {
                        _logger.LogError(webEx, "Cheque posting timeout for cheque: {ChequeNumber}", chequeNumber);
                    }
                    else
                    {
                        _logger.LogError(webEx, "Cheque posting web error for cheque: {ChequeNumber}", chequeNumber);
                    }

                    chequeData.PostingTime = transTime;
                    chequeData.ApproverId = approverId;
                    chequeData.AuthorizerId = authorizerId;
                    chequeData.status = "IP";
                    chequeData.stan = stan;
                    chequeData.TrProcORRecTime = DateTime.Now.ToString("yy-MM-dd:hh:mm:tt");
                    await _context.SaveChangesAsync();

                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Cheque posting error for cheque: {ChequeNumber}", chequeNumber);

                    chequeData.PostingTime = transTime;
                    chequeData.ApproverId = approverId;
                    chequeData.AuthorizerId = authorizerId;
                    chequeData.status = "IP";
                    chequeData.stan = stan;
                    chequeData.TrProcORRecTime = DateTime.Now.ToString("yy-MM-dd:hh:mm:tt");
                    await _context.SaveChangesAsync();

                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MBLPendingChequePostingAsync");
                throw;
            }
        }

        private async Task<string> GetCreditAccountAsync(string cycleCode, string hubCode)
        {
            var hub = await _context.Hub.FirstOrDefaultAsync(x => x.Code == hubCode);
            if (hub == null) return string.Empty;

            return cycleCode switch
            {
                "02" => hub.CrAccNormal ?? string.Empty,
                "05" => hub.CrAccSameDay ?? string.Empty,
                "01" => hub.CrAccIntercity ?? string.Empty,
                "20" => hub.CrAccDollar ?? string.Empty,
                _ => string.Empty
            };
        }

        public async Task<MBLPOLodgementExt?> MBLPOEncashmentAsync(long id, string stan, string accountNumber, string chequeNumber, string amount, string cycleCode, string hubCode, string senderBankCode, string instrumentNo, string approverId, string authorizerId)
        {
            try
            {
                var chequeData = await _context.chequedepositInformation.FindAsync(id);
                if (chequeData == null)
                {
                    _logger.LogError("Cheque deposit not found: {Id}", id);
                    return null;
                }

                var baseUrl = _configuration["CoreBanking:POEncBaseUrl"];
                int maxRetryAttempts = int.Parse(_configuration["CoreBanking:MaxRetryAttempts"] ?? "3");

                // Remove leading zero from cheque number if present
                var processedChequeNumber = chequeNumber.StartsWith("0") ? chequeNumber.Substring(1) : chequeNumber;

                // Determine instrument type
                string poSpec;
                if (instrumentNo == "020" || instrumentNo == "000" || instrumentNo == "009")
                {
                    poSpec = "PO";
                }
                else
                {
                    poSpec = "CDR";
                }

                // Get credit account based on cycle code
                var creditAccount = await GetCreditAccountAsync(cycleCode, hubCode);

                var amountDecimal = Convert.ToDecimal(amount);
                var userId = _configuration["CoreBanking:UserID"];
                var password = _configuration["CoreBanking:Password"];
                var channelType = _configuration["CoreBanking:PoEncChannelType"];
                var channelSubType = _configuration["CoreBanking:PoEncChannelSubType"];
                var transactionType = _configuration["CoreBanking:PoEncTransactionType"];
                var transactionSubType = _configuration["CoreBanking:PoEncTransactionSubType"];
                var function = _configuration["CoreBanking:poEncFunction"];
                var pan = _configuration["CoreBanking:poEncPAN"];

                var postingBranch = accountNumber.Length > 12 ? accountNumber.Remove(0, 12) : accountNumber;

                var requestXml = $@"<MBLPayOrderEncashment>
                    <UserID>{userId}</UserID>
                    <Password>{password}</Password>
                    <ChannelType>{channelType}</ChannelType>
                    <ChannelSubType>{channelSubType}</ChannelSubType>
                    <TransactionType>{transactionType}</TransactionType>
                    <TransactionSubType>{transactionSubType}</TransactionSubType>
                    <TranDateAndTime>{DateTime.Now:yyyy-MM-ddThh:mm:ss}</TranDateAndTime>
                    <Function>{function}</Function>
                    <HostData>
                        <PayOrderEncashmentRequest>
                            <STAN>{stan}</STAN>
                            <PAN>{pan}</PAN>
                            <PostingBranch>{postingBranch}</PostingBranch>
                            <InstType>{poSpec}</InstType>
                            <InstNo>{processedChequeNumber}</InstNo>
                            <Amount>{amountDecimal}</Amount>
                            <Bank>{senderBankCode}</Bank>
                            <Return></Return>
                            <Reason></Reason>
                            <CreditAccount>{creditAccount}</CreditAccount>
                            <Reserved></Reserved>
                        </PayOrderEncashmentRequest>
                    </HostData>
                </MBLPayOrderEncashment>";

                _logger.LogInformation("PO Encashment Request: {Request}", requestXml);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(bytes, 0, bytes.Length);
                }

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                var responseText = await reader.ReadToEndAsync();
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(responseText);

                                var jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                                _logger.LogInformation("PO Encashment Response: {Response}", jsonText);

                                var finalRes = JsonConvert.DeserializeObject<MBLPOLodgementExt>(jsonText);

                                if (finalRes?.MBLPayOrderEncashmentResponse?.hostData?.HostPayOrderEncashmentResponse != null)
                                {
                                    if (finalRes.MBLPayOrderEncashmentResponse.hostData.HostPayOrderEncashmentResponse.HostCode == "00")
                                    {
                                        chequeData.status = "A";
                                        chequeData.CoreFTId = finalRes.MBLPayOrderEncashmentResponse.hostData.HostPayOrderEncashmentResponse.TransactionID;
                                        chequeData.ApproverId = approverId;
                                        chequeData.AuthorizerId = authorizerId;
                                        chequeData.TrProcORRecTime = DateTime.Now.ToString("yy-MM-dd:hh:mm:tt");

                                        int retryAttempt = 0;
                                        bool updateSuccess = false;

                                        while (!updateSuccess && retryAttempt < maxRetryAttempts)
                                        {
                                            try
                                            {
                                                await _context.SaveChangesAsync();
                                                updateSuccess = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.LogError(ex, "Retry {Attempt} for PO encashment {ChequeNumber}", retryAttempt + 1, chequeNumber);
                                                retryAttempt++;
                                                await Task.Delay(1000);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        chequeData.status = "RE";
                                        chequeData.PostRestriction = finalRes.MBLPayOrderEncashmentResponse.hostData.HostPayOrderEncashmentResponse.HostCode;
                                        await _context.SaveChangesAsync();
                                    }

                                    return finalRes;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in PO encashment for cheque: {ChequeNumber}", chequeNumber);
                    throw;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MBLPOEncashmentAsync");
                throw;
            }
        }
    }
}
