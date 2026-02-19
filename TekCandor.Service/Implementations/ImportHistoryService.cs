using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class ImportHistoryService : IImportHistoryService
    {
        private readonly IImportHistoryRepository _repository;

        public ImportHistoryService(IImportHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ImportHistoryResponse>> GetImportHistoryAsync(
            ImportHistoryRequest request,
            CancellationToken cancellationToken = default)
        {
            var (data, totalCount) = await _repository.GetImportHistoryAsync(
                request.Date, request.Page, request.PageSize,
                request.SortColumn, request.SortDirection,
                cancellationToken);

            var mapped = data.Select(x => new ImportHistoryResponse
            {
                Id = x.Id,
                FileName = x.FileName,
                Date = x.Date,
                TotalRecords = x.TotalRecords,
                SuccessfullRecords = x.SuccessfullRecords,
                FailureRecords = x.FailureRecords
            });

            return new PagedResult<ImportHistoryResponse>
            {
                Items = mapped,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ImportHistoryResponse>> GetManualImportHistoryAsync(
            ImportHistoryRequest request,
            CancellationToken cancellationToken = default)
        {
            var (data, totalCount) = await _repository.GetManualImportHistoryAsync(
                request.Date, request.Page, request.PageSize,
                request.SortColumn, request.SortDirection,
                cancellationToken);

            var mapped = data.Select(x => new ImportHistoryResponse
            {
                Id = x.Id,
                FileName = x.FileName,
                Date = x.Date,
                TotalRecords = x.TotalRecords,
                SuccessfullRecords = x.SuccessfullRecords,
                FailureRecords = x.FailureRecords
            });

            return new PagedResult<ImportHistoryResponse>
            {
                Items = mapped,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ImportDataDetailResponse>> GetImportDataDetailAsync(
            long importDataId,
            ImportDataDetailRequest request,
            CancellationToken cancellationToken = default)
        {
            var (data, totalCount) = await _repository.GetImportDataDetailAsync(
                importDataId, request.Page, request.PageSize,
                request.SortColumn, request.SortDirection,
                cancellationToken);

            return new PagedResult<ImportDataDetailResponse>
            {
                Items = data.Select(x => new ImportDataDetailResponse
                {
                    Id = x.Id,
                    ImportDataId = x.ImportDataId,
                    Data = x.Data,
                    Date = x.Date,
                    Error = x.Error,
                    ErrorDescription = x.ErrorDescription
                }),
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PagedResult<ImportDataDetailResponse>> GetManualImportDataDetailAsync(
            long importDataId,
            ImportDataDetailRequest request,
            CancellationToken cancellationToken = default)
        {
            var (data, totalCount) = await _repository.GetManualImportDataDetailAsync(
                importDataId, request.Page, request.PageSize,
                request.SortColumn, request.SortDirection,
                cancellationToken);

            return new PagedResult<ImportDataDetailResponse>
            {
                Items = data.Select(x => new ImportDataDetailResponse
                {
                    Id = x.Id,
                    ImportDataId = x.Manual_ImportDataId,
                    Data = x.Data,
                    Date = x.Date,
                    Error = x.Error,
                    ErrorDescription = x.ErrorDescription
                }),
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
