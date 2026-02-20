using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IImportHistoryRepository
    {
        Task<(IEnumerable<ImportData> Data, int TotalCount)> GetImportHistoryAsync(
            DateTime? date, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default);

        Task<(IEnumerable<Manual_ImportData> Data, int TotalCount)> GetManualImportHistoryAsync(
            DateTime? date, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default);

        Task<(IEnumerable<ImportDataDetail> Data, int TotalCount)> GetImportDataDetailAsync(
            long importDataId, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default);

        Task<(IEnumerable<Manual_ImportDataDetails> Data, int TotalCount)> GetManualImportDataDetailAsync(
            long importDataId, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default);
    }
}
