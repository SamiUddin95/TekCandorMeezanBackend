using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Models;

namespace TekCandor.Repository.Interfaces
{
    public interface IChequeDepositRepository
    {
        Task<bool> AddAsync(ChequedepositInfo chequeDeposit);
        Task<bool> AddRangeAsync(IEnumerable<ChequedepositInfo> chequeDeposits);
        Task<string> GetHubCodeAsync(string receiverBranchCode);

        //List
        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetChequeDepositListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           string branchOrHub,
           string? hubIds,
           string? branchCodes,
           CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetCallbackListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           string branchOrHub,
           string? hubIds,
           string? branchCodes,
           CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetReturnListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           string branchOrHub,
           string? hubIds,
           string? branchCodes,
           CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetBranchReturnListAsync(
          ChequeDepositListRequestDTO request,
          long userId,
          string branchOrHub,
          string? hubIds,
          string? branchCodes,
          CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetApprovedListAsync(
          ChequeDepositListRequestDTO request,
          long userId,
          string branchOrHub,
          string? hubIds,
          string? branchCodes,
          CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetUnAuthorizedListAsync(
          ChequeDepositListRequestDTO request,
          long userId,
          string branchOrHub,
          string? hubIds,
          string? branchCodes,
          CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetRejectListAsync(
          ChequeDepositListRequestDTO request,
          long userId,
          string branchOrHub,
          string? hubIds,
          string? branchCodes,
          CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetInProcessListAsync(
         ChequeDepositListRequestDTO request,
         long userId,
         string branchOrHub,
         string? hubIds,
         string? branchCodes,
         CancellationToken cancellationToken = default);

        Task<ChequeDeposit?> GetByIdAsync(long id);


    }
}
