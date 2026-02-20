using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities.Data;
using TekCandor.Service.Interfaces;

namespace TekCandor.Service.Implementations
{
    public class UserContextService : IUserContextService
    {
        private readonly AppDbContext _context;

        public UserContextService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserScope> GetUserScopeAsync(long userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new { u.BranchorHub, u.HubIds, u.BranchIds })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return new UserScope("BranchWise", null, null);

            string? branchCodes = null;

            if (user.BranchorHub == "HubWise" && !string.IsNullOrWhiteSpace(user.HubIds))
            {
                var hubIdList = user.HubIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(id => long.TryParse(id, out var v) ? v : (long?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .ToList();

                var codes = await _context.Branch
                    .AsNoTracking()
                    .Where(b => hubIdList.Contains(b.HubId))
                    .Select(b => b.NIFTBranchCode)
                    .Where(c => c != null)
                    .ToListAsync(cancellationToken);

                branchCodes = codes.Count > 0
                    ? string.Join(",", codes.Select(c => $"'{c}'"))
                    : null;
            }
            else if (!string.IsNullOrWhiteSpace(user.BranchIds))
            {
                var branchIdList = user.BranchIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(id => long.TryParse(id, out var v) ? v : (long?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .ToList();

                var codes = await _context.Branch
                    .AsNoTracking()
                    .Where(b => branchIdList.Contains(b.Id))
                    .Select(b => b.NIFTBranchCode)
                    .Where(c => c != null)
                    .ToListAsync(cancellationToken);

                branchCodes = codes.Count > 0
                    ? string.Join(",", codes.Select(c => $"'{c}'"))
                    : null;
            }

            return new UserScope(
                user.BranchorHub ?? "BranchWise",
                user.HubIds,
                branchCodes);
        }
    }
}
