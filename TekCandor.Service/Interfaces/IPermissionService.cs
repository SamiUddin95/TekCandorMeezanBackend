using System.Threading.Tasks;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IPermissionService
    {
        Task<PagedResult<PermissionDTO>> GetAllAsync(int pageNumber, int pageSize);
    }
}
