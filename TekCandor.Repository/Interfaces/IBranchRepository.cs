
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IBranchRepository
    {
        Task<IQueryable<Branch>> GetAllQueryableAsync();
        Branch Add(Branch branch);
        Branch? GetById(long id);
        Branch? Update(Branch branch);
        bool SoftDelete(long id);
    }
}
