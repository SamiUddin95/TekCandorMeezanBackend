using System.Threading;
using System.Threading.Tasks;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IImportHistoryService
    {
        Task<PagedResult<ImportHistoryResponse>> GetImportHistoryAsync(
            ImportHistoryRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ImportHistoryResponse>> GetManualImportHistoryAsync(
            ImportHistoryRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ImportDataDetailResponse>> GetImportDataDetailAsync(
            long importDataId,
            ImportDataDetailRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ImportDataDetailResponse>> GetManualImportDataDetailAsync(
            long importDataId,
            ImportDataDetailRequest request,
            CancellationToken cancellationToken = default);
    }
}
