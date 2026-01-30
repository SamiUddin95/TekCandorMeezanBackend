using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class BranchRepository :IBranchRepository
    {
        private readonly AppDbContext _context;
        public BranchRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Branch> GetAll()
        {
            return _context.Branch;
        }
        public Branch Add(Branch branch)
        {
            branch.Id = Guid.NewGuid();
            branch.CreatedDateTime = DateTime.Now;
            _context.Branch.Add(branch);
            _context.SaveChanges();
            return branch;
        }

        public Branch? GetById(Guid id)
        {
            return _context.Branch.FirstOrDefault(b => b.Id == id);
        }

        public Branch? Update (Branch branch)
        {
            var existing = _context.Branch.FirstOrDefault(b => b.Id == branch.Id);
            if (existing == null) return null;
            existing.NIFT = branch.NIFT;
            existing.Code = branch.Code;
            existing.NIFTBranchCode = branch.NIFTBranchCode;
            existing.Name= branch.Name;
            existing.HubId = branch.HubId;
            existing.Version= branch.Version;
            existing.IsNew = branch.IsNew;
            existing.IsDeleted = branch.IsDeleted;
            existing.CreatedUser= branch.CreatedUser;
            existing.CreatedDateTime = branch.CreatedDateTime;
            existing.ModifiedUser= branch.ModifiedUser;
            existing.ModifiedDateTime = DateTime.Now;
            existing.Email1 = branch.Email1;
            existing.Email2 = branch.Email2;
            existing.Email3 = branch.Email3;
            _context.SaveChanges();
            return existing;
        }

        public bool SoftDelete(Guid id)
        {
            var existing = _context.Branch.FirstOrDefault(b => b.Id == id);
            if (existing == null) return false;
            if (existing.IsDeleted) return true;
            existing.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }
    }
}
