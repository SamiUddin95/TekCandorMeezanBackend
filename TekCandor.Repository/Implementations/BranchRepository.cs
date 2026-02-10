
using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class BranchRepository : IBranchRepository
    {
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Branch>> GetAllQueryableAsync()
        {
            return _context.Branch.AsNoTracking();
        }

        public Branch Add(Branch branch)
        {
            _context.Branch.Add(branch);
            _context.SaveChanges();
            return branch;
        }

        public Branch? GetById(long id)
        {
            return _context.Branch.FirstOrDefault(b => b.Id == id);
        }

        public Branch? Update(Branch branch)
        {
            var existing = _context.Branch.FirstOrDefault(b => b.Id == branch.Id);
            if (existing == null) return null;

            existing.Code = branch.Code;
            existing.NIFTBranchCode = branch.NIFTBranchCode;
            existing.Name = branch.Name;
            existing.HubId = branch.HubId;
            existing.IsDeleted = branch.IsDeleted;
            existing.UpdatedBy = branch.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;
            existing.Email1 = branch.Email1;
            existing.Email2 = branch.Email2;
            existing.Email3 = branch.Email3;

            _context.SaveChanges();
            return existing;
        }

        public bool SoftDelete(long id)
        {
            var existing = _context.Branch.FirstOrDefault(b => b.Id == id);
            if (existing == null) return false;
            if (existing.IsDeleted) return true;

            existing.IsDeleted = true;
            existing.UpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
    }
}
