using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ImportHistoryRepository : IImportHistoryRepository
    {
        private readonly AppDbContext _context;

        private static readonly HashSet<string> AllowedColumns = new(StringComparer.OrdinalIgnoreCase)
        {
            "Date", "FileName", "TotalRecords", "SuccessfullRecords", "FailureRecords"
        };

        public ImportHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<ImportData> Data, int TotalCount)> GetImportHistoryAsync(
            DateTime? date, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default)
        {
            var query = _context.ImportData.Where(x => !x.IsDeleted);

            if (date.HasValue)
                query = query.Where(x => x.Date.Date == date.Value.Date);

            var totalCount = await query.CountAsync(cancellationToken);

            var safeSort = AllowedColumns.Contains(sortColumn ?? "") ? sortColumn : "Date";
            var ascending = string.Equals(sortDirection, "ASC", StringComparison.OrdinalIgnoreCase);

            query = (safeSort, ascending) switch
            {
                ("FileName",    true)  => query.OrderBy(x => x.FileName),
                ("FileName",    false) => query.OrderByDescending(x => x.FileName),
                ("TotalRecords", true) => query.OrderBy(x => x.TotalRecords),
                ("TotalRecords", false)=> query.OrderByDescending(x => x.TotalRecords),
                ("SuccessfullRecords", true)  => query.OrderBy(x => x.SuccessfullRecords),
                ("SuccessfullRecords", false) => query.OrderByDescending(x => x.SuccessfullRecords),
                ("FailureRecords", true)  => query.OrderBy(x => x.FailureRecords),
                ("FailureRecords", false) => query.OrderByDescending(x => x.FailureRecords),
                (_, true)  => query.OrderBy(x => x.Date),
                _          => query.OrderByDescending(x => x.Date),
            };

            var skip = (page - 1) * pageSize;
            var data = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return (data, totalCount);
        }

        public async Task<(IEnumerable<Manual_ImportData> Data, int TotalCount)> GetManualImportHistoryAsync(
            DateTime? date, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Manual_ImportData.Where(x => !x.IsDeleted);

            if (date.HasValue)
                query = query.Where(x => x.Date.Date == date.Value.Date);

            var totalCount = await query.CountAsync(cancellationToken);

            var safeSort = AllowedColumns.Contains(sortColumn ?? "") ? sortColumn : "Date";
            var ascending = string.Equals(sortDirection, "ASC", StringComparison.OrdinalIgnoreCase);

            query = (safeSort, ascending) switch
            {
                ("FileName",    true)  => query.OrderBy(x => x.FileName),
                ("FileName",    false) => query.OrderByDescending(x => x.FileName),
                ("TotalRecords", true) => query.OrderBy(x => x.TotalRecords),
                ("TotalRecords", false)=> query.OrderByDescending(x => x.TotalRecords),
                ("SuccessfullRecords", true)  => query.OrderBy(x => x.SuccessfullRecords),
                ("SuccessfullRecords", false) => query.OrderByDescending(x => x.SuccessfullRecords),
                ("FailureRecords", true)  => query.OrderBy(x => x.FailureRecords),
                ("FailureRecords", false) => query.OrderByDescending(x => x.FailureRecords),
                (_, true)  => query.OrderBy(x => x.Date),
                _          => query.OrderByDescending(x => x.Date),
            };

            var skip = (page - 1) * pageSize;
            var data = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return (data, totalCount);
        }

        public async Task<(IEnumerable<ImportDataDetail> Data, int TotalCount)> GetImportDataDetailAsync(
            long importDataId, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default)
        {
            var allowedDetailColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Date", "Data", "Error", "ErrorDescription"
            };

            var query = _context.ImportDataDetail
                .Where(x => x.ImportDataId == importDataId && !x.IsDeleted);

            var totalCount = await query.CountAsync(cancellationToken);

            var safeSort = allowedDetailColumns.Contains(sortColumn ?? "") ? sortColumn : "Date";
            var ascending = string.Equals(sortDirection, "ASC", StringComparison.OrdinalIgnoreCase);

            query = (safeSort, ascending) switch
            {
                ("Data",             true)  => query.OrderBy(x => x.Data),
                ("Data",             false) => query.OrderByDescending(x => x.Data),
                ("Error",            true)  => query.OrderBy(x => x.Error),
                ("Error",            false) => query.OrderByDescending(x => x.Error),
                ("ErrorDescription", true)  => query.OrderBy(x => x.ErrorDescription),
                ("ErrorDescription", false) => query.OrderByDescending(x => x.ErrorDescription),
                (_,                  true)  => query.OrderBy(x => x.Date),
                _                          => query.OrderByDescending(x => x.Date),
            };

            var skip = (page - 1) * pageSize;
            var data = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return (data, totalCount);
        }

        public async Task<(IEnumerable<Manual_ImportDataDetails> Data, int TotalCount)> GetManualImportDataDetailAsync(
            long importDataId, int page, int pageSize, string? sortColumn, string? sortDirection,
            CancellationToken cancellationToken = default)
        {
            var allowedDetailColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Date", "Data", "Error", "ErrorDescription"
            };

            var query = _context.Manual_ImportDataDetails
                .Where(x => x.Manual_ImportDataId == importDataId && !x.IsDeleted);

            var totalCount = await query.CountAsync(cancellationToken);

            var safeSort = allowedDetailColumns.Contains(sortColumn ?? "") ? sortColumn : "Date";
            var ascending = string.Equals(sortDirection, "ASC", StringComparison.OrdinalIgnoreCase);

            query = (safeSort, ascending) switch
            {
                ("Data",             true)  => query.OrderBy(x => x.Data),
                ("Data",             false) => query.OrderByDescending(x => x.Data),
                ("Error",            true)  => query.OrderBy(x => x.Error),
                ("Error",            false) => query.OrderByDescending(x => x.Error),
                ("ErrorDescription", true)  => query.OrderBy(x => x.ErrorDescription),
                ("ErrorDescription", false) => query.OrderByDescending(x => x.ErrorDescription),
                (_,                  true)  => query.OrderBy(x => x.Date),
                _                          => query.OrderByDescending(x => x.Date),
            };

            var skip = (page - 1) * pageSize;
            var data = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return (data, totalCount);
        }
    }
}
