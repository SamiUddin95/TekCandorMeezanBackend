using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IManualImportDataRepository
    {
        Task<Manual_ImportData> GetByFileNameAsync(string fileName);
        Task<long> AddAsync(Manual_ImportData importData);
        Task<bool> AddDetailRangeAsync(IEnumerable<Manual_ImportDataDetails> details);
        Task<bool> UpdateStatisticsAsync(long importDataId, int successCount, int failureCount);
    }
}
