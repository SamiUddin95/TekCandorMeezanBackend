using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace TekCandor.Repository.Implementations
{
    public class LiveMonitoringRepository : ILiveMonitoringRepository
    {
        private readonly AppDbContext _context;
        public LiveMonitoringRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<ChequedepositInfo>> GetAllQueryableAsync()
        {
            return _context.chequedepositInformation.AsNoTracking();
        }
    }
}