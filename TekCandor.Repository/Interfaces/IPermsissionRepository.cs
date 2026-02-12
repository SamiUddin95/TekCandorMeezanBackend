using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllQueryableAsync();
    }
}
