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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Models;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class ChequeDepositService: IChequeDepositService
    {
        private readonly AppDbContext _context;
        private readonly IChequeDepositRepository _repository;
        private readonly IUserContextService _userContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChequeDepositService> _logger;
        private readonly ICoreBankingService _coreBankingService;

        public ChequeDepositService(
            IChequeDepositRepository repository,
            IUserContextService userContext,
            AppDbContext context,
            IConfiguration configuration,
            ILogger<ChequeDepositService> logger,
            ICoreBankingService coreBankingService)
        {
            _repository = repository;
            _userContext = userContext;
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _coreBankingService = coreBankingService;
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetChequeDepositListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetChequeDepositListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetCallbackListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetCallbackListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetReturnListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetReturnListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetBranchReturnListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetBranchReturnListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetApprovedListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetApprovedListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetUnAuthorizedListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetUnAuthorizedListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetRejectListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetRejectListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ChequeDepositListResponseDTO>> GetInProcessListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default)
        {
            // Resolve branch/hub scope for this user (replaces old GetBranchCodes + HubWise logic)
            var userInfo = await _userContext.GetUserScopeAsync(userId, cancellationToken);

            var (data, totalCount) = await _repository.GetInProcessListAsync(
                request,
                userId,
                userInfo.BranchOrHub,
                userInfo.HubIds,
                userInfo.BranchCodes,
                cancellationToken);

            return new PagedResult<ChequeDepositListResponseDTO>
            {
                Items = data,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<ChequeDepositResponse?> GetByIdAsync(long id)
        {
            var cheque = await _context.chequedepositInformation
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.status,
                    x.SequenceNumber,
                    x.AccountNumber,
                    x.AccountTitle,
                    x.Amount,
                    x.AccountBalance,
                    x.poStatus,
                    x.ErrorInFields,
                    x.ChequeNumber
                })
                .FirstOrDefaultAsync();

            if (cheque == null)
                return null;

            var response = new ChequeDepositResponse
            {
                Id = cheque.Id,
                Date = cheque.Date,
                StatusText = cheque.status switch
                {
                    "P" => "Pending Status",
                    "AR" => "Approved Reversed",
                    _ => cheque.status ?? string.Empty
                },
                AccountNumber = cheque.AccountNumber ?? string.Empty,
                ChequeNumber = cheque.ChequeNumber ?? string.Empty,
                AccountTitle = cheque.AccountTitle ?? string.Empty,
                Amount = cheque.Amount ?? 0,
                ErrorInFields = cheque.ErrorInFields ?? string.Empty,
                ImagePath = _configuration["FileLocations:NiftImages"]
            };

            // Images
            if (!string.IsNullOrEmpty(cheque.SequenceNumber))
            {
                response.ImgF = cheque.SequenceNumber + "F";
                response.ImgR = cheque.SequenceNumber + "B";
                response.ImgU = cheque.SequenceNumber + "U";
            }

            // Signatures (optimized query)
            response.Signature = await _context.Signatures
                .AsNoTracking()
                .Where(x => x.AccountNumber == cheque.AccountNumber)
                .Select(x => x.Sign)
                .ToArrayAsync();

            // Balance formatting
            if (!string.IsNullOrEmpty(cheque.AccountBalance) &&
                decimal.TryParse(cheque.AccountBalance, out decimal balance))
            {
                response.AccountBalance = (balance / 100).ToString("#,##.##");
            }
            else
            {
                response.AccountBalance = string.Empty;
            }

            return response;
        }

        public async Task<ChequeDepositCallbackResponse?> GetCallBackEditAsync(long id)
        {
            var cheque = await _context.chequedepositInformation
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.SequenceNumber,
                    x.AccountNumber,
                    x.AccountTitle,
                    x.Amount,
                    x.AccountBalance,
                    x.poStatus,
                    x.ErrorInFields,
                    x.ReceiverBranchCode,
                    x.ChequeNumber,
                    x.InstrumentNo,
                    x.TransactionCode,
                    x.Remarks,
                    x.Returnreasone,
                    x.Currency,
                    x.Callback,
                    x.Callbacksend
                })
                .FirstOrDefaultAsync();

            if (cheque == null)
                return null;

            var response = new ChequeDepositCallbackResponse
            {
                Id = cheque.Id,
                Date = cheque.Date,
                Status = "Call Back",
                AccountNumber = cheque.AccountNumber ?? string.Empty,
                AccountTitle = cheque.AccountTitle ?? string.Empty,
                Amount = cheque.Amount ?? 0,
                PoStatus = cheque.poStatus ?? string.Empty,
                ErrorFieldsName = cheque.ErrorInFields,
                ReceiverBranchCode = cheque.ReceiverBranchCode ?? string.Empty,
                ChequeNumber = cheque.ChequeNumber ?? string.Empty,
                InstrumentNo = cheque.InstrumentNo ?? string.Empty,
                SequenceNumber = cheque.SequenceNumber ?? string.Empty,
                TransactionCode = cheque.TransactionCode ?? string.Empty,
                Remarks = cheque.Remarks ?? string.Empty,
                Returnreasone = cheque.Returnreasone ?? string.Empty,
                Currency = cheque.Currency ?? string.Empty,
                Callback = cheque.Callback,
                CBCStatus = cheque.Callbacksend,
                ImagePath = _configuration["FileLocations:NiftImages"]
            };

            // Images
            if (!string.IsNullOrEmpty(cheque.SequenceNumber))
            {
                response.ImgF = cheque.SequenceNumber + "F";
                response.ImgR = cheque.SequenceNumber + "B";
                response.ImgU = cheque.SequenceNumber + "U";
            }

            // Check if account is PO account (starts with 00017571 or 00017574)
            bool isPOAccount = cheque.AccountNumber != null && 
                              (cheque.AccountNumber.StartsWith("00017571") || 
                               cheque.AccountNumber.StartsWith("00017574"));

            if (!isPOAccount)
            {
                // For non-PO accounts, get signatures
                response.Signature = await _context.Signatures
                    .AsNoTracking()
                    .Where(x => x.AccountNumber == cheque.AccountNumber)
                    .Select(x => x.Sign)
                    .ToArrayAsync();

                // Balance formatting
                if (!string.IsNullOrEmpty(cheque.AccountBalance) &&
                    decimal.TryParse(cheque.AccountBalance, out decimal balance))
                {
                    response.AccountBalance = (balance / 100).ToString("#,##.##");
                }
                else
                {
                    response.AccountBalance = string.Empty;
                }
            }
            else
            {
                // For PO accounts, set beneficiary detail from account title
                response.BeneficiaryDetail = cheque.AccountTitle ?? string.Empty;
            }

            return response;
        }

        public async Task<ChequeDepositBranchReturnResponse?> GetBranchReturnEditAsync(long id)
        {
            var cheque = await _context.chequedepositInformation
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.SequenceNumber,
                    x.AccountNumber,
                    x.AccountTitle,
                    x.Amount,
                    x.AccountBalance,
                    x.poStatus,
                    x.ErrorInFields,
                    x.ReceiverBranchCode,
                    x.ChequeNumber,
                    x.InstrumentNo,
                    x.TransactionCode,
                    x.Remarks,
                    x.Returnreasone,
                    x.Currency,
                    x.Callbacksend,
                    x.BranchRemarks
                })
                .FirstOrDefaultAsync();

            if (cheque == null)
                return null;

            var response = new ChequeDepositBranchReturnResponse
            {
                Id = cheque.Id,
                Date = cheque.Date,
                Status = "Rejected",
                AccountNumber = cheque.AccountNumber ?? string.Empty,
                AccountTitle = cheque.AccountTitle ?? string.Empty,
                Amount = cheque.Amount ?? 0,
                PoStatus = cheque.poStatus ?? string.Empty,
                ErrorFieldsName = cheque.ErrorInFields,
                ReceiverBranchCode = cheque.ReceiverBranchCode ?? string.Empty,
                ChequeNumber = cheque.ChequeNumber ?? string.Empty,
                InstrumentNo = cheque.InstrumentNo ?? string.Empty,
                SequenceNumber = cheque.SequenceNumber ?? string.Empty,
                TransactionCode = cheque.TransactionCode ?? string.Empty,
                Remarks = cheque.Remarks ?? string.Empty,
                Currency = cheque.Currency ?? string.Empty,
                Callbacksend = cheque.Callbacksend,
                BranchRemarks = cheque.BranchRemarks ?? string.Empty,
                RejectedReasonsByCCU = cheque.Returnreasone ?? string.Empty,
                ImagePath = _configuration["FileLocations:NiftImages"]
            };

            // Images
            if (!string.IsNullOrEmpty(cheque.SequenceNumber))
            {
                response.ImgF = cheque.SequenceNumber + "F";
                response.ImgR = cheque.SequenceNumber + "B";
                response.ImgU = cheque.SequenceNumber + "U";
            }

            // Check if account is PO account (starts with 00017571 or 00017574)
            bool isPOAccount = cheque.AccountNumber != null && 
                              (cheque.AccountNumber.StartsWith("00017571") || 
                               cheque.AccountNumber.StartsWith("00017574"));

            if (!isPOAccount)
            {
                // For non-PO accounts, get signatures
                response.Signature = await _context.Signatures
                    .AsNoTracking()
                    .Where(x => x.AccountNumber == cheque.AccountNumber)
                    .Select(x => x.Sign)
                    .ToArrayAsync();

                // Balance formatting
                if (!string.IsNullOrEmpty(cheque.AccountBalance) &&
                    decimal.TryParse(cheque.AccountBalance, out decimal balance))
                {
                    response.AccountBalance = (balance / 100).ToString("#,##.##");
                }
                else
                {
                    response.AccountBalance = string.Empty;
                }

                // Get return reason name if return reason code exists
                if (!string.IsNullOrEmpty(cheque.Returnreasone))
                {
                    var returnReason = await _context.ReturnReason
                        .AsNoTracking()
                        .Where(r => r.Code == cheque.Returnreasone && !r.IsDeleted)
                        .Select(r => r.Name)
                        .FirstOrDefaultAsync();

                    response.RejectedReasonsByCCU = returnReason ?? cheque.Returnreasone;
                }
            }
            else
            {
                // For PO accounts, set beneficiary detail from account title
                response.BeneficiaryDetail = cheque.AccountTitle ?? string.Empty;
                response.RejectedReasonsByCCU = cheque.Returnreasone ?? string.Empty;
            }

            return response;
        }

        public async Task<ChequeDepositAuthorizerResponse?> GetAuthorizerEditAsync(long id)
        {
            var cheque = await _context.chequedepositInformation
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.status,
                    x.SequenceNumber,
                    x.AccountNumber,
                    x.AccountTitle,
                    x.Amount,
                    x.AccountBalance,
                    x.poStatus,
                    x.ErrorInFields,
                    x.ReceiverBranchCode,
                    x.ChequeNumber,
                    x.InstrumentNo,
                    x.TransactionCode,
                    x.Remarks,
                    x.Returnreasone,
                    x.Currency,
                    x.Callbacksend,
                    x.CycleCode,
                    x.HubCode
                })
                .FirstOrDefaultAsync();

            if (cheque == null)
                return null;

            // Map status code to text
            string statusText = cheque.status switch
            {
                "U" => "Un Authorized",
                "RE" => "Rejected",
                _ => cheque.status ?? string.Empty
            };

            var response = new ChequeDepositAuthorizerResponse
            {
                Id = cheque.Id,
                Date = cheque.Date,
                Status = statusText,
                AccountNumber = cheque.AccountNumber ?? string.Empty,
                AccountTitle = cheque.AccountTitle ?? string.Empty,
                Amount = cheque.Amount ?? 0,
                PoStatus = cheque.poStatus ?? string.Empty,
                ErrorFieldsName = cheque.ErrorInFields,
                ReceiverBranchCode = cheque.ReceiverBranchCode ?? string.Empty,
                ChequeNumber = cheque.ChequeNumber ?? string.Empty,
                InstrumentNo = cheque.InstrumentNo ?? string.Empty,
                SequenceNumber = cheque.SequenceNumber ?? string.Empty,
                TransactionCode = cheque.TransactionCode ?? string.Empty,
                Remarks = cheque.Remarks ?? string.Empty,
                Returnreasone = cheque.Returnreasone ?? string.Empty,
                Currency = cheque.Currency ?? string.Empty,
                Callbacksend = cheque.Callbacksend,
                CycleCode = cheque.CycleCode ?? string.Empty,
                HubCode = cheque.HubCode ?? string.Empty,
                ImagePath = _configuration["FileLocations:NiftImages"]
            };

            // Images
            if (!string.IsNullOrEmpty(cheque.SequenceNumber))
            {
                response.ImgF = cheque.SequenceNumber + "F";
                response.ImgR = cheque.SequenceNumber + "B";
                response.ImgU = cheque.SequenceNumber + "U";
            }

            // Check if account is PO account (starts with 00017571 or 00017574)
            bool isPOAccount = cheque.AccountNumber != null && 
                              (cheque.AccountNumber.StartsWith("00017571") || 
                               cheque.AccountNumber.StartsWith("00017574"));

            if (!isPOAccount)
            {
                // For non-PO accounts, get signatures
                response.Signature = await _context.Signatures
                    .AsNoTracking()
                    .Where(x => x.AccountNumber == cheque.AccountNumber)
                    .Select(x => x.Sign)
                    .ToArrayAsync();

                // Balance formatting
                if (!string.IsNullOrEmpty(cheque.AccountBalance) &&
                    decimal.TryParse(cheque.AccountBalance, out decimal balance))
                {
                    response.AccountBalance = (balance / 100).ToString("#,##.##");
                }
                else
                {
                    response.AccountBalance = string.Empty;
                }
            }
            else
            {
                // For PO accounts, set beneficiary detail from account title
                response.BeneficiaryDetail = cheque.AccountTitle ?? string.Empty;
            }

            return response;
        }

        public async Task<ChequeDepositRejectResponse?> GetRejectEditAsync(long id)
        {
            var cheque = await _context.chequedepositInformation
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.SequenceNumber,
                    x.AccountNumber,
                    x.AccountTitle,
                    x.Amount,
                    x.AccountBalance,
                    x.poStatus,
                    x.ErrorInFields,
                    x.ReceiverBranchCode,
                    x.ChequeNumber,
                    x.InstrumentNo,
                    x.TransactionCode,
                    x.Remarks,
                    x.Returnreasone,
                    x.Currency,
                    x.Callbacksend,
                    x.CycleCode,
                    x.HubCode,
                    x.Callback
                })
                .FirstOrDefaultAsync();

            if (cheque == null)
                return null;

            var response = new ChequeDepositRejectResponse
            {
                Id = cheque.Id,
                Date = cheque.Date,
                Status = "Rejected",
                AccountNumber = cheque.AccountNumber ?? string.Empty,
                AccountTitle = cheque.AccountTitle ?? string.Empty,
                Amount = cheque.Amount ?? 0,
                PoStatus = cheque.poStatus ?? string.Empty,
                ErrorFieldsName = cheque.ErrorInFields,
                ReceiverBranchCode = cheque.ReceiverBranchCode ?? string.Empty,
                ChequeNumber = cheque.ChequeNumber ?? string.Empty,
                InstrumentNo = cheque.InstrumentNo ?? string.Empty,
                SequenceNumber = cheque.SequenceNumber ?? string.Empty,
                TransactionCode = cheque.TransactionCode ?? string.Empty,
                Remarks = cheque.Remarks ?? string.Empty,
                Returnreasone = cheque.Returnreasone ?? string.Empty,
                Currency = cheque.Currency ?? string.Empty,
                Callbacksend = cheque.Callbacksend,
                CycleCode = cheque.CycleCode ?? string.Empty,
                HubCode = cheque.HubCode ?? string.Empty,
                Callback = cheque.Callback,
                ImagePath = _configuration["FileLocations:NiftImages"]
            };

            // Images
            if (!string.IsNullOrEmpty(cheque.SequenceNumber))
            {
                response.ImgF = cheque.SequenceNumber + "F";
                response.ImgR = cheque.SequenceNumber + "B";
                response.ImgU = cheque.SequenceNumber + "U";
            }

            // Check if account is PO account (starts with 00017571 or 00017574)
            bool isPOAccount = cheque.AccountNumber != null && 
                              (cheque.AccountNumber.StartsWith("00017571") || 
                               cheque.AccountNumber.StartsWith("00017574"));

            if (!isPOAccount)
            {
                // For non-PO accounts, get signatures
                response.Signature = await _context.Signatures
                    .AsNoTracking()
                    .Where(x => x.AccountNumber == cheque.AccountNumber)
                    .Select(x => x.Sign)
                    .ToArrayAsync();

                // Balance formatting
                if (!string.IsNullOrEmpty(cheque.AccountBalance) &&
                    decimal.TryParse(cheque.AccountBalance, out decimal balance))
                {
                    response.AccountBalance = (balance / 100).ToString("#,##.##");
                }
                else
                {
                    response.AccountBalance = string.Empty;
                }
            }
            else
            {
                // For PO accounts, set beneficiary detail from account title
                response.BeneficiaryDetail = cheque.AccountTitle ?? string.Empty;
            }

            return response;
        }

        public async Task<bool> GetSignatureAsync(long id, string accountNumber, string chequeNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(chequeNumber))
                {
                    _logger.LogWarning("Account number or cheque number is empty");
                    return false;
                }

                // Update cheque deposit with new account and cheque number
                var chequeDeposit = await _context.chequedepositInformation.FindAsync(id);
                if (chequeDeposit == null)
                {
                    _logger.LogWarning("Cheque deposit not found: {Id}", id);
                    return false;
                }

                chequeDeposit.OldAccount = chequeDeposit.AccountNumber;
                chequeDeposit.AccountNumber = accountNumber;
                chequeDeposit.OldChequeNo = chequeDeposit.ChequeNumber;
                chequeDeposit.ChequeNumber = chequeNumber;

                if (accountNumber != "0000000000000000")
                {
                    await MBLSignatureAsync(id, accountNumber);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSignatureAsync");
                return false;
            }
        }

        private async Task MBLSignatureAsync(long id, string criteriaValue)
        {
            try
            {
                _logger.LogInformation("Signature Start {CriteriaValue} {Time}", criteriaValue, DateTime.UtcNow);

                var sigBaseUrl = _configuration["Signature:BaseUrl"];
                var userId = _configuration["CoreBanking:UserID"];
                var password = _configuration["CoreBanking:Password"];
                var channelType = _configuration["CoreBanking:ChannelType"];
                var channelSubType = _configuration["CoreBanking:ChannelSubType"];
                var transactionType = _configuration["CoreBanking:TransactionType"];
                var transactionSubType = _configuration["CoreBanking:TransactionSubType"];
                var sigFunction = _configuration["Signature:Function"];
                var thumbprint = _configuration["CoreBanking:ThumbPrint"];

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count == 0)
                {
                    store.Close();
                    _logger.LogError("Certificate not found");
                    throw new Exception("Certificate not found");
                }

                X509Certificate2 certificate = certificates[0];
                store.Close();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sigBaseUrl);
                request.ClientCertificates.Add(certificate);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Timeout = 100000;

                string criteriaValueTrimmed = criteriaValue.Length > 6 ? criteriaValue.Remove(0, 6) : criteriaValue;

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
                            <criteriaValue>{criteriaValueTrimmed}</criteriaValue>
                        </MBLT24SignatureReq>
                    </HostData>
                </MBLT24Signature>";

                XmlDocument reqDoc = new XmlDocument();
                reqDoc.LoadXml(signatureRequest);

                using (Stream stream = request.GetRequestStream())
                {
                    reqDoc.Save(stream);
                }

                _logger.LogInformation("Signature Request: {Request}", reqDoc.InnerXml);

                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    XmlDocument responseDoc = new XmlDocument();
                    responseDoc.LoadXml(responseText);

                    var jsonText = JsonConvert.SerializeXmlNode(responseDoc, Newtonsoft.Json.Formatting.Indented);
                    _logger.LogInformation("Signature Response: {Response}", jsonText);

                    var responseObj = JObject.Parse(jsonText);
                    var signatureData = responseObj.SelectToken("MBLT24SignatureResponse.hostData.Signature");

                    if (signatureData is JArray signatureArray)
                    {
                        await ProcessMultipleSignaturesAsync(signatureArray, criteriaValue);
                    }
                    else if (signatureData is JObject singleSignature)
                    {
                        await ProcessSingleSignatureAsync(singleSignature, criteriaValue);
                    }
                }

                // Update isEditing flag
                var cheques = await _context.chequedepositInformation
                    .Where(x => x.AccountNumber == criteriaValue)
                    .ToListAsync();

                foreach (var cheque in cheques)
                {
                    cheque.isEditing = true;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Signature Finish {CriteriaValue} {Time}", criteriaValue, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MBLSignatureAsync");
                throw;
            }
        }

        private async Task ProcessMultipleSignaturesAsync(JArray signatureArray, string criteriaValue)
        {
            try
            {
                // Get existing signatures from database
                var existingSignatures = await _context.Signatures
                    .Where(x => x.AccountNumber == criteriaValue)
                    .ToListAsync();

                var signatureList = signatureArray.ToObject<List<PathItem>>();
                var matched = new List<string>();
                var notMatched = new List<PathItem>();

                var existingFileNames = existingSignatures
                    .Select(x => Path.GetFileName(Encoding.UTF8.GetString(x.Sign ?? Array.Empty<byte>())))
                    .ToList();

                // Compare signatures
                foreach (var item in signatureList)
                {
                    string fileName = Path.GetFileName(item.Path);
                    string normalizedFileName = NormalizeFileName(fileName);

                    if (existingFileNames.Any(x => string.Equals(x, normalizedFileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        matched.Add(normalizedFileName);
                    }
                    else
                    {
                        notMatched.Add(item);
                    }
                }

                // Delete signatures that are no longer in the response
                var toDelete = existingFileNames.Except(matched, StringComparer.OrdinalIgnoreCase).ToList();
                foreach (var fileName in toDelete)
                {
                    var sigToDelete = existingSignatures.FirstOrDefault(x =>
                        string.Equals(Path.GetFileName(Encoding.UTF8.GetString(x.Sign ?? Array.Empty<byte>())), fileName, StringComparison.OrdinalIgnoreCase));

                    if (sigToDelete != null)
                    {
                        _context.Signatures.Remove(sigToDelete);

                        // Delete physical file
                        var localPath = _configuration["Signature:LocalPath"];
                        var fullPath = Path.Combine(localPath, fileName);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // Download new signatures
                if (notMatched.Count > 0)
                {
                    await DownloadSignaturesViaSftpAsync(notMatched, criteriaValue);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing multiple signatures");
                throw;
            }
        }

        private async Task ProcessSingleSignatureAsync(JObject singleSignature, string criteriaValue)
        {
            try
            {
                var signature = singleSignature.ToObject<PathItem>();
                if (signature != null && !string.IsNullOrEmpty(Path.GetExtension(signature.Path)))
                {
                    var signatures = new List<PathItem> { signature };
                    await DownloadSignaturesViaSftpAsync(signatures, criteriaValue);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing single signature");
                throw;
            }
        }

        private async Task DownloadSignaturesViaSftpAsync(List<PathItem> signatures, string criteriaValue)
        {
            try
            {
                var sftpHost = _configuration["Signature:SftpHost"];
                var sftpPort = int.Parse(_configuration["Signature:SftpPort"] ?? "22");
                var sftpUsername = _configuration["Signature:SftpUsername"];
                var sftpPassword = _configuration["Signature:SftpPassword"];
                var localPath = _configuration["Signature:LocalPath"];
                var sigImagesPath = _configuration["Signature:ImagesPath"];

                using (var sftpClient = new SftpClient(sftpHost, sftpPort, sftpUsername, sftpPassword))
                {
                    sftpClient.Connect();

                    foreach (var item in signatures)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(Path.GetExtension(item.Path)))
                                continue;

                            string fileName = Path.GetFileName(item.Path);
                            string remotePath = item.Path.Replace("/IBM/WebSphere/AppServer/profiles/AppSrv01/installedApps/was21Cell01/R16_TLIVE21_war.ear/R16_TLIVE21.war/im.images", "");

                            fileName = NormalizeFileName(fileName);

                            string localFilePath = Path.Combine(localPath, fileName);

                            // Download file
                            using (var fileStream = System.IO.File.OpenWrite(localFilePath))
                            {
                                sftpClient.DownloadFile(remotePath, fileStream);
                            }

                            _logger.LogInformation("Downloaded signature: {FileName} to {LocalPath}", fileName, localFilePath);

                            // Insert signature record
                            var signature = new Signature
                            {
                                AccountNumber = criteriaValue,
                                Sign = Encoding.UTF8.GetBytes(sigImagesPath + fileName),
                                CreatedOn = DateTime.Now,
                                IsDeleted = false
                            };

                            _context.Signatures.Add(signature);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error downloading signature: {Path}", item.Path);
                        }
                    }

                    sftpClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DownloadSignaturesViaSftpAsync");
                throw;
            }
        }

        private string NormalizeFileName(string fileName)
        {
            if (fileName.EndsWith(".00.JPG", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".01.JPG", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".08.JPG", StringComparison.OrdinalIgnoreCase))
            {
                var removedot = fileName.Replace(".", "");
                removedot = Regex.Replace(removedot, "jpg$", "", RegexOptions.IgnoreCase);
                return removedot + ".JPG";
            }
            else if (fileName.EndsWith(".00.jpg", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".01.jpg", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".08.jpg", StringComparison.OrdinalIgnoreCase))
            {
                var removedot = fileName.Replace(".", "");
                removedot = Regex.Replace(removedot, "jpg$", "", RegexOptions.IgnoreCase);
                return removedot + ".JPG";
            }
            else if (fileName.EndsWith(".00", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".01", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".08", StringComparison.OrdinalIgnoreCase))
            {
                var removedot = fileName.Replace(".", "");
                return removedot + ".JPG";
            }

            return fileName;
        }

        private class PathItem
        {
            public string Path { get; set; } = string.Empty;
        }

        public async Task<PendingToInprocessResponse> PendingToInprocessAsync(List<long> selectedIds)
        {
            try
            {
                var chequeNumbers = new List<string>();
                int successCount = 0;

                if (selectedIds == null || selectedIds.Count == 0)
                {
                    return new PendingToInprocessResponse
                    {
                        SuccessCount = 0,
                        ChequeNumbers = string.Empty,
                        Message = "No cheques selected"
                    };
                }

                foreach (long id in selectedIds)
                {
                    var chequeData = await _context.chequedepositInformation.FindAsync(id);
                    
                    if (chequeData != null && chequeData.status == "P")
                    {
                        chequeNumbers.Add(chequeData.ChequeNumber ?? string.Empty);
                        successCount++;
                        
                        chequeData.status = "IP";
                        _context.chequedepositInformation.Update(chequeData);
                    }
                }

                if (successCount > 0)
                {
                    await _context.SaveChangesAsync();
                }

                var chequeNumbersStr = string.Join(", ", chequeNumbers);
                var message = successCount > 0 
                    ? $"{successCount} Cheques of Cheque Number {chequeNumbersStr} sent to In-Process Cheques"
                    : "Only Invalid Record will be Export";

                return new PendingToInprocessResponse
                {
                    SuccessCount = successCount,
                    ChequeNumbers = chequeNumbersStr,
                    Message = message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingToInprocessAsync");
                throw;
            }
        }

        public async Task<decimal> GetLimitAsync(long userId)
        {
            try
            {
                var limit = await (from u in _context.Users
                                   join sgu in _context.SecurityGroup_User on u.Id equals sgu.UserId
                                   join sg in _context.Group on sgu.SecurityGroupId equals sg.Id
                                   where u.Id == userId
                                   select sg.UpperLimit)
                            .FirstOrDefaultAsync();
                return limit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLimitAsync");
                return 0;
            }
        }

        public async Task<PendingApproveSelectedResponse> PendingApproveSelectedAsync(List<long> selectedIds, long userId, string loginName)
        {
            try
            {
                var chequeNumbers = new List<string>();
                int successCount = 0;

                if (selectedIds == null || selectedIds.Count == 0)
                {
                    return new PendingApproveSelectedResponse
                    {
                        SuccessCount = 0,
                        ChequeNumbers = string.Empty,
                        Message = "No cheques selected"
                    };
                }

                var userLimit = await GetLimitAsync(userId);

                foreach (long id in selectedIds)
                {
                    try
                    {
                        var chequeData = await _context.chequedepositInformation.FindAsync(id);
                        if (chequeData == null) continue;

                        // Check if amount exceeds user limit
                        if (chequeData.Amount > userLimit)
                        {
                            chequeData.ApproverId = loginName;
                            chequeData.status = "U";
                            _context.chequedepositInformation.Update(chequeData);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (chequeData.AccountNumber != "0000000000000000")
                            {
                                string stan;
                                if (chequeData.status == "AR")
                                {
                                    Random generator = new Random();
                                    stan = generator.Next(0, 1000000).ToString("D6");
                                }
                                else
                                {
                                    stan = chequeData.stan ?? string.Empty;
                                }

                                // Call core banking service to post the cheque
                                var result = await _coreBankingService.MBLPendingChequePostingAsync(
                                    chequeData.Id,
                                    stan,
                                    chequeData.AccountNumber ?? string.Empty,
                                    chequeData.ChequeNumber ?? string.Empty,
                                    chequeData.Currency ?? string.Empty,
                                    chequeData.Amount.ToString(),
                                    chequeData.CycleCode ?? string.Empty,
                                    chequeData.HubCode ?? string.Empty,
                                    loginName,
                                    loginName
                                );

                                if (result?.MBLFT?.hostData?.hostFTResponse?.HostCode == "00")
                                {
                                    chequeNumbers.Add(chequeData.ChequeNumber ?? string.Empty);
                                    successCount++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing cheque {Id}", id);
                    }
                }

                var chequeNumbersStr = string.Join(", ", chequeNumbers);
                var message = successCount > 0
                    ? $"{successCount} Cheques approved successfully"
                    : "No cheques were approved";

                return new PendingApproveSelectedResponse
                {
                    SuccessCount = successCount,
                    ChequeNumbers = chequeNumbersStr,
                    Message = message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingApproveSelectedAsync");
                throw;
            }
        }

        public async Task<PendingChequeApproveResponse> PendingChequeApproveAsync(long id, string? accountNumber, string? chequeNumber, long userId, string loginName)
        {
            try
            {
                var chequeData = await _context.chequedepositInformation.FindAsync(id);
                if (chequeData == null)
                {
                    return new PendingChequeApproveResponse
                    {
                        Success = false,
                        Message = "Cheque not found",
                        Status = "failed"
                    };
                }

                // Check if it's a PO account (exclude from posting)
                if (chequeData.AccountNumber != null && 
                    (chequeData.AccountNumber.StartsWith("00017571") || chequeData.AccountNumber.StartsWith("00017574")))
                {
                    return new PendingChequeApproveResponse
                    {
                        Success = false,
                        Message = "PO accounts cannot be posted",
                        Status = "failed"
                    };
                }

                // Get user limit
                var userLimit = await GetLimitAsync(userId);

                // Check if amount exceeds user limit
                if (chequeData.Amount > userLimit)
                {
                    chequeData.ApproverId = loginName;
                    chequeData.status = "U";
                    _context.chequedepositInformation.Update(chequeData);
                    await _context.SaveChangesAsync();

                    return new PendingChequeApproveResponse
                    {
                        Success = false,
                        Message = "Your limit mismatch it goes to Authorizer",
                        Status = "U"
                    };
                }

                // Update account number and cheque number if provided
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    chequeData.AccountNumber = accountNumber;
                }
                if (!string.IsNullOrEmpty(chequeNumber))
                {
                    chequeData.ChequeNumber = chequeNumber;
                }

                // Check for duplicate posting
                if (chequeData.PostingTime != null && chequeData.status != "AR")
                {
                    chequeData.status = "A";
                    await _context.SaveChangesAsync();

                    return new PendingChequeApproveResponse
                    {
                        Success = false,
                        Message = "Cheque Already Posted",
                        Status = "A"
                    };
                }

                // Generate STAN
                string stan;
                if (chequeData.status == "AR")
                {
                    Random generator = new Random();
                    stan = generator.Next(0, 1000000).ToString("D6");
                }
                else
                {
                    stan = chequeData.stan ?? string.Empty;
                }

                // Post to core banking
                var result = await _coreBankingService.MBLPendingChequePostingAsync(
                    chequeData.Id,
                    stan,
                    chequeData.AccountNumber ?? string.Empty,
                    chequeData.ChequeNumber ?? string.Empty,
                    chequeData.Currency ?? string.Empty,
                    chequeData.Amount.ToString(),
                    chequeData.CycleCode ?? string.Empty,
                    chequeData.HubCode ?? string.Empty,
                    loginName,
                    loginName
                );

                if (result?.MBLFT?.hostData?.hostFTResponse != null)
                {
                    if (result.MBLFT.hostData.hostFTResponse.HostCode == "00")
                    {
                        return new PendingChequeApproveResponse
                        {
                            Success = true,
                            Message = "Cheque approved successfully",
                            Status = "A"
                        };
                    }
                    else
                    {
                        return new PendingChequeApproveResponse
                        {
                            Success = false,
                            Message = result.MBLFT.hostData.hostFTResponse.HostDesc ?? "Posting failed",
                            Status = "RE"
                        };
                    }
                }

                return new PendingChequeApproveResponse
                {
                    Success = false,
                    Message = "Core banking service error",
                    Status = "failed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingChequeApproveAsync");
                return new PendingChequeApproveResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Status = "failed"
                };
            }
        }

        public async Task<PendingPOApproveResponse> PendingPOApproveAsync(long id, long userId, string loginName)
        {
            try
            {
                var chequeData = await _context.chequedepositInformation.FindAsync(id);
                if (chequeData == null)
                {
                    return new PendingPOApproveResponse
                    {
                        Success = false,
                        Message = "Cheque not found",
                        Status = "failed"
                    };
                }

                // Validate instrument type (020, 009, 000, 080)
                if (chequeData.InstrumentNo != "020" && 
                    chequeData.InstrumentNo != "009" && 
                    chequeData.InstrumentNo != "000" && 
                    chequeData.InstrumentNo != "080")
                {
                    return new PendingPOApproveResponse
                    {
                        Success = false,
                        Message = "Invalid instrument type for PO approval",
                        Status = "failed"
                    };
                }

                // Get user limit
                var userLimit = await GetLimitAsync(userId);

                // Check if amount exceeds user limit
                if (chequeData.Amount > userLimit)
                {
                    chequeData.ApproverId = loginName;
                    chequeData.status = "U";
                    _context.chequedepositInformation.Update(chequeData);
                    await _context.SaveChangesAsync();

                    return new PendingPOApproveResponse
                    {
                        Success = false,
                        Message = "Your limit mismatch it goes to Authorizer",
                        Status = "U"
                    };
                }

                // Check for duplicate posting
                var duplicateCheck = await _context.chequedepositInformation
                    .FirstOrDefaultAsync(x => x.ChequeNumber == chequeData.ChequeNumber);

                if (duplicateCheck != null && 
                    (duplicateCheck.PostingTime != null || duplicateCheck.TrProcORRecTime != null))
                {
                    chequeData.status = "A";
                    await _context.SaveChangesAsync();

                    return new PendingPOApproveResponse
                    {
                        Success = false,
                        Message = "Cheque Already Posted",
                        Status = "A"
                    };
                }

                // Post to core banking
                var result = await _coreBankingService.MBLPOEncashmentAsync(
                    chequeData.Id,
                    chequeData.stan ?? string.Empty,
                    chequeData.AccountNumber ?? string.Empty,
                    chequeData.ChequeNumber ?? string.Empty,
                    chequeData.Amount.ToString(),
                    chequeData.CycleCode ?? string.Empty,
                    chequeData.HubCode ?? string.Empty,
                    chequeData.SenderBankCode ?? string.Empty,
                    chequeData.InstrumentNo ?? string.Empty,
                    loginName,
                    loginName
                );

                if (result?.MBLPayOrderEncashmentResponse?.hostData?.HostPayOrderEncashmentResponse != null)
                {
                    if (result.MBLPayOrderEncashmentResponse.hostData.HostPayOrderEncashmentResponse.HostCode == "00")
                    {
                        return new PendingPOApproveResponse
                        {
                            Success = true,
                            Message = "PO/CDR approved successfully",
                            Status = "A"
                        };
                    }
                    else
                    {
                        return new PendingPOApproveResponse
                        {
                            Success = false,
                            Message = result.MBLPayOrderEncashmentResponse.hostData.HostPayOrderEncashmentResponse.HostCode ?? "Posting failed",
                            Status = "RE"
                        };
                    }
                }

                return new PendingPOApproveResponse
                {
                    Success = false,
                    Message = "Core banking service error",
                    Status = "failed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingPOApproveAsync");
                return new PendingPOApproveResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Status = "failed"
                };
            }
        }
    }
}
