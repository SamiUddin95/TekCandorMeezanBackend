using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Entities.Data;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TekCandor.Service.Implementations
{
    public class FilterService : IFilterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IHubRepository _hubRepository;
        private readonly ICycleRepository _cycleRepository;
        private readonly AppDbContext _context;

        public FilterService(
            IUserRepository userRepository,
            IBranchRepository branchRepository,
            IHubRepository hubRepository,
            ICycleRepository cycleRepository,
            AppDbContext context)
        {
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _hubRepository = hubRepository;
            _cycleRepository = cycleRepository;
            _context = context;
        }

        public async Task<BranchFilterResponse> GetBranchFilterForUserAsync(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var response = new BranchFilterResponse
            {
                FilterType = user.BranchorHub ?? "BranchWise"
            };

            var query = await _branchRepository.GetAllQueryableAsync();
            query = query.Where(b => !b.IsDeleted);

            if (user.BranchorHub == "HubWise")
            {
                // For HubWise users, return all branches
                response.Branches = await query
                    .OrderBy(b => b.Name)
                    .Select(b => new BranchFilterDTO
                    {
                        Name = b.Name ?? string.Empty,
                        Code = b.Code ?? string.Empty
                    })
                    .ToListAsync();
            }
            else
            {
                // For BranchWise users, filter by user's BranchIds
                if (!string.IsNullOrEmpty(user.BranchIds))
                {
                    var branchIdList = user.BranchIds
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => long.TryParse(id.Trim(), out var bid) ? bid : 0)
                        .Where(id => id > 0)
                        .ToList();

                    response.Branches = await query
                        .Where(b => branchIdList.Contains(b.Id))
                        .OrderBy(b => b.Name)
                        .Select(b => new BranchFilterDTO
                        {
                            Name = b.Name ?? string.Empty,
                            Code = b.Code ?? string.Empty
                        })
                        .ToListAsync();
                }
            }

            return response;
        }

        public async Task<HubFilterResponse> GetHubFilterForUserAsync(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var response = new HubFilterResponse
            {
                FilterType = user.BranchorHub ?? "BranchWise"
            };

            var query = await _hubRepository.GetAllQueryableAsync();
            query = query.Where(h => !h.IsDeleted);

            // Filter by user's HubIds (comma-separated list)
            if (!string.IsNullOrEmpty(user.HubIds))
            {
                var hubIdList = user.HubIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => long.TryParse(id.Trim(), out var hid) ? hid : 0)
                    .Where(id => id > 0)
                    .ToList();

                response.Hubs = await query
                    .Where(h => hubIdList.Contains(h.Id))
                    .OrderBy(h => h.Name)
                    .Select(h => new HubFilterDTO
                    {
                        Name = (h.Name ?? string.Empty) + "-" + (h.Code ?? string.Empty),
                        Code = h.Code ?? string.Empty
                    })
                    .ToListAsync();
            }

            return response;
        }

        public async Task<StatusFilterResponse> GetStatusFilterAsync()
        {
            var response = new StatusFilterResponse();

            response.Statuses = await _context.ClearingStatuses
                .AsNoTracking()
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Text)
                .Select(s => new FilterOptionDTO
                {
                    Text = s.Text ?? string.Empty,
                    Value = s.Value ?? string.Empty
                })
                .ToListAsync();

            return response;
        }

        public async Task<InstrumentFilterResponse> GetInstrumentFilterAsync()
        {
            var response = new InstrumentFilterResponse();

            response.Instruments = await _context.Instruments
                .AsNoTracking()
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.Name)
                .Select(i => new FilterOptionDTO
                {
                    Text = i.Name ?? string.Empty,
                    Value = i.Code ?? string.Empty
                })
                .ToListAsync();

            return response;
        }

        public async Task<CycleFilterResponse> GetCycleFilterAsync()
        {
            var response = new CycleFilterResponse();

            var query = await _cycleRepository.GetAllQueryableAsync();
            response.Cycles = await query
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .Select(c => new FilterOptionDTO
                {
                    Text = c.Name ?? string.Empty,
                    Value = c.Code ?? string.Empty
                })
                .ToListAsync();

            return response;
        }

        public Task<ServiceRunFilterResponse> GetServiceRunFilterAsync()
        {
            var response = new ServiceRunFilterResponse();

            // ServiceRun is a boolean field, return True/False options
            response.Options = new List<FilterOptionDTO>
            {
                new FilterOptionDTO { Text = "True", Value = "true" },
                new FilterOptionDTO { Text = "False", Value = "false" }
            };

            return Task.FromResult(response);
        }
    }
}
