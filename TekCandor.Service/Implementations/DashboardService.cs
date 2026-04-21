using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;


namespace TekCandor.Service.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        private readonly AppDbContext _context;

        public DashboardService(IDashboardRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<DashboardResponseDTO> GetDashboardAsync(string hubIds, string branchOrHub)
        {
            var query = await _repository.GetAllQueryableAsync();

            query = query.Where(x => x.Date.Date == DateTime.Today && !x.IsDeleted);

            var hubIdList = hubIds.Split(',')
                .Select(long.Parse)
                .ToList();

            var branchCodes = await _context.Branch
                .Where(b => hubIdList.Contains(b.HubId))
                .Select(b => b.Code)
                .ToListAsync();

            query = query.Where(x => branchCodes.Contains(x.ReceiverBranchCode));

            
            var normal = await query
                .Where(x => x.CycleCode == "02")
                .GroupBy(x => x.status)
                .Select(g => new DashboardDTO
                {
                    Status =
                        g.Key == "A" ? "Approved" :
                        g.Key == "R" ? "Return" :
                        g.Key == "AR" ? "Approved Reversed" :
                        g.Key == "RR" ? "Return Reversed" :
                        g.Key == "U" ? "Un Authorized" :
                        g.Key == "RE" ? "System Rejected" :
                        g.Key == "P" ? "Pending" :
                        g.Key == "MA" ? "Manual Approved" :
                        g.Key == "IP" ? "In Process" :
                        g.Key == "MAR" ? "Manual Approved Reversed" :
                        g.Key,

                    Cheques = g.Count(),
                    Amount = g.Sum(x => x.Amount) ?? 0
                })
                .ToListAsync();

            var sameDay = await query
                .Where(x => x.CycleCode == "05")
                .GroupBy(x => x.status)
                .Select(g => new DashboardDTO
                {
                    Status =
                        g.Key == "A" ? "Approved" :
                        g.Key == "R" ? "Return" :
                        g.Key == "AR" ? "Approved Reversed" :
                        g.Key == "RR" ? "Return Reversed" :
                        g.Key == "U" ? "Un Authorized" :
                        g.Key == "RE" ? "System Rejected" :
                        g.Key == "P" ? "Pending" :
                        g.Key == "MA" ? "Manual Approved" :
                        g.Key == "IP" ? "In Process" :
                        g.Key == "MAR" ? "Manual Approved Reversed" :
                        g.Key,

                    Cheques = g.Count(),
                    Amount = g.Sum(x => x.Amount) ?? 0
                })
                .ToListAsync();
            
            var normalTotal = await query
                .Where(x => x.CycleCode == "02")
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalCount = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount) ?? 0
                })
                .FirstOrDefaultAsync();

            
            var sameDayTotal = await query
                .Where(x => x.CycleCode == "05")
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalCount = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount) ?? 0
                })
                .FirstOrDefaultAsync();

           
            var combinedTotal = await query
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalCount = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount) ?? 0
                })
                .FirstOrDefaultAsync();

            return new DashboardResponseDTO
            {
                Normal = normal,
                SameDay = sameDay,
                NormalTotalCount = normalTotal?.TotalCount ?? 0,
                NomalTotalAmount = normalTotal?.TotalAmount ?? 0,

                SameDayTotalCount = sameDayTotal?.TotalCount ?? 0,
                SameDayTotalAmount = sameDayTotal?.TotalAmount ?? 0,

                BothTotalCount = combinedTotal?.TotalCount ?? 0,
                BothTotalAmount = combinedTotal?.TotalAmount ?? 0
            };
        }
    }
}
