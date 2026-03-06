using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
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

        public ChequeDepositService(
            IChequeDepositRepository repository,
            IUserContextService userContext,
            AppDbContext context,
            IConfiguration configuration)
        {
            _repository = repository;
            _userContext = userContext;
            _context = context;
            _configuration = configuration;
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
    }
}
