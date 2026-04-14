using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;

namespace TekCandor.Repository.Implementations.Outward
{
    public class DepositorTypeRepository : IDepositorTypeRepository
    {
        private readonly AppDbContext _context;

        public DepositorTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepositorType>> GetAllAsync()
        {
            return await _context.DepositorType.ToListAsync();
        }
    }
}
