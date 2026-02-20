using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ChequeDepositRepository : IChequeDepositRepository
    {
        private readonly AppDbContext _context;

        public ChequeDepositRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(ChequedepositInfo chequeDeposit)
        {
            _context.chequedepositInformation.Add(chequeDeposit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<ChequedepositInfo> chequeDeposits)
        {
            await _context.chequedepositInformation.AddRangeAsync(chequeDeposits);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> GetHubCodeAsync(string receiverBranchCode)
        {
            var branch = await _context.Branch
                .FirstOrDefaultAsync(b => b.Code == receiverBranchCode && !b.IsDeleted);

            if (branch == null) return string.Empty;

            var hub = await _context.Hub
                .FirstOrDefaultAsync(h => h.Id == branch.HubId && !h.IsDeleted);

            return hub?.Code ?? string.Empty;
        }
    }
}
