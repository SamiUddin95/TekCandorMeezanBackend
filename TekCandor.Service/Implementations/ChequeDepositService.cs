using Microsoft.EntityFrameworkCore;
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

        public ChequeDepositService(
            IChequeDepositRepository repository,
            IUserContextService userContext,
            AppDbContext context)
        {
            _repository = repository;
            _userContext = userContext;
            _context = context;
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
                ErrorInFields = cheque.ErrorInFields ?? string.Empty
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
    }
}
