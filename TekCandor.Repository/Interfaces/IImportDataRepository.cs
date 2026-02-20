using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IImportDataRepository
    {
        Task<ImportData> GetByFileNameAsync(string fileName);
        Task<long> AddAsync(ImportData importData);
        Task<bool> AddDetailAsync(ImportDataDetail detail);
        Task<bool> AddDetailRangeAsync(IEnumerable<ImportDataDetail> details);
        Task<bool> UpdateStatisticsAsync(long importDataId, int successCount, int failureCount);
    }
}
