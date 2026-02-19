using System.Collections.Generic;
using System.Threading.Tasks;

namespace TekCandor.Service.Interfaces
{
    public interface IChequeDepositImportService
    {
        Task<bool> ImportSFTPFilesAsync(string hostName, int port, string username, string password, string remoteLocation, string localLocation);
        Task<List<string>> ProcessImportedFilesAsync(string folderPath, string callbackLimit);
        Task<List<string>> ProcessManualImportAsync(string folderPath, string callbackLimit, string processedFolderPath);
        Task<string> GetHubCodeAsync(string receiverBranchCode);
    }
}
