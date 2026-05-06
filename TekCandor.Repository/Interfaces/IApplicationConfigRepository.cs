using System.Threading.Tasks;

namespace TekCandor.Repository.Interfaces
{
    public interface IApplicationConfigRepository
    {
        Task<string?> GetValueByKeyAsync(string key);
        Task<bool> UpdateValueByKeyAsync(string key, string value, string updatedBy);
    }
}
