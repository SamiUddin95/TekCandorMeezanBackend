using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Models;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class ChequeDepositService: IChequeDepositService
    {
        private readonly IChequeDepositRepository _repository;
        private readonly IUserContextService _userContext; 

        public ChequeDepositService(
            IChequeDepositRepository repository,
            IUserContextService userContext)
        {
            _repository = repository;
            _userContext = userContext;
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
    }
}
