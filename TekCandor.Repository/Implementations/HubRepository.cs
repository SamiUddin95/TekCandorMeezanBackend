using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class HubRepository : IHubRepository
    {
        private readonly AppDbContext _context;

        public HubRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Hub>> GetAllQueryableAsync()
        {
            return _context.Hub.AsNoTracking();
        }



        public async Task<Hub?> GetByIdAsync(long id)
        {
            return await _context.Hub.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(Hub hub)
        {
            await _context.Hub.AddAsync(hub);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
