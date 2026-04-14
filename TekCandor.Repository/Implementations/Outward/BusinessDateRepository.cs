using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;

namespace TekCandor.Repository.Implementations.Outward
{
    public class BusinessDateRepository : IBusinessDateRepository
    {
        private readonly AppDbContext _context;

        public BusinessDateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessDate> CreateAsync(BusinessDate businessDate)
        {
            businessDate.StartedAt = DateTime.Now;
            _context.BusinessDate.Add(businessDate);
            await _context.SaveChangesAsync();
            return businessDate;
        }

        public async Task<List<BusinessDate>> GetAllAsync()
        {
            return await _context.BusinessDate.ToListAsync();
        }

    }
}
