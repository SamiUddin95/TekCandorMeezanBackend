using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces.Outward;

namespace TekCandor.Repository.Implementations.Outward
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppDbContext _context;

        public CurrencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Currency>> GetAllAsync()
        {
            return await _context.Currency
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }
    }
}
