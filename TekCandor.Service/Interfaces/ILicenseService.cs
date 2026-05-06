using System.Threading.Tasks;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface ILicenseService
    {
        Task<LicenseStatusDTO> GetLicenseStatusAsync();
        Task<bool> IsLicenseValidAsync();
        Task<bool> UpdateLicenseAsync(string encryptedKey, string updatedBy);
    }
}
