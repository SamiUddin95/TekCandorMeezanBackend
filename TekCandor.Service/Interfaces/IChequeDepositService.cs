using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Models;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IChequeDepositService
    {
        Task<PagedResult<ChequeDepositListResponseDTO>> GetChequeDepositListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default);
    }
}
