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
        Task<bool> GetSignatureAsync(long id, string accountNumber, string chequeNumber);
        Task<PendingToInprocessResponse> PendingToInprocessAsync(List<long> selectedIds);
        Task<decimal> GetLimitAsync(long userId);
        Task<PendingApproveSelectedResponse> PendingApproveSelectedAsync(List<long> selectedIds, long userId, string loginName);
        Task<PendingChequeApproveResponse> PendingChequeApproveAsync(long id, string? accountNumber, string? chequeNumber, long userId, string loginName);
        Task<PendingPOApproveResponse> PendingPOApproveAsync(long id, long userId, string loginName);
    }
}
