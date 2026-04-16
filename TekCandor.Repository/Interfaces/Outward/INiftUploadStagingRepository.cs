using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface INiftUploadStagingRepository
    {
        Task<NiftUploadStaging> CreateAsync(NiftUploadStaging niftUpload);
        Task<List<NiftUploadStaging>> CreateBulkAsync(List<NiftUploadStaging> niftUploads);
        Task<bool> UpdateIsProcessedAsync(long id, bool isProcessed);
    }
}
