using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;
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

        Task<PagedResult<ChequeDepositListResponseDTO>> GetCallbackListAsync(
            ChequeDepositListRequestDTO request,
            long userId,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ChequeDepositListResponseDTO>> GetReturnListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           CancellationToken cancellationToken = default);

        Task<PagedResult<ChequeDepositListResponseDTO>> GetBranchReturnListAsync(
         ChequeDepositListRequestDTO request,
         long userId,
         CancellationToken cancellationToken = default);

        Task<PagedResult<ChequeDepositListResponseDTO>> GetApprovedListAsync(
        ChequeDepositListRequestDTO request,
        long userId,
        CancellationToken cancellationToken = default);

        Task<PagedResult<ChequeDepositListResponseDTO>> GetUnAuthorizedListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           CancellationToken cancellationToken = default);

        Task<PagedResult<ChequeDepositListResponseDTO>> GetRejectListAsync(
          ChequeDepositListRequestDTO request,
          long userId,
          CancellationToken cancellationToken = default);

        Task<PagedResult<ChequeDepositListResponseDTO>> GetInProcessListAsync(
        ChequeDepositListRequestDTO request,
        long userId,
        CancellationToken cancellationToken = default);

        Task<ChequeDepositResponse?> GetByIdAsync(long id);
        Task<ChequeDepositCallbackResponse?> GetCallBackEditAsync(long id);
        Task<ChequeDepositBranchReturnResponse?> GetBranchReturnEditAsync(long id);
        Task<ChequeDepositAuthorizerResponse?> GetAuthorizerEditAsync(long id);
        Task<ChequeDepositRejectResponse?> GetRejectEditAsync(long id);
    }
}
