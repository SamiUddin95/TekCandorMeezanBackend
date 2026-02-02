using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
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

        public IEnumerable<Branch> GetAllQueryable()
        {
            return _context.Branch.AsNoTracking();
        }

        public async Task<Branch?> GetByIdAsync(long id)
        {
            return await _context.Branch.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Branch branch)
        {
            await _context.Branch.AddAsync(branch);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
