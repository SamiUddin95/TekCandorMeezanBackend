using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class LiveMonitoringService : ILiveMonitoringService
    {
        private readonly ILiveMonitoringRepository _repository;
        private readonly AppDbContext _context;

        public LiveMonitoringService(ILiveMonitoringRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<LiveMonitoringDTO>> GetMonitoringDataAsync()
        {
            var query = await _repository.GetAllQueryableAsync();
            query = query.Where(x => x.Date.Date == DateTime.Today && !x.IsDeleted);

            var result = await query
             .GroupBy(x => x.serviceRun)
             .Select(g => new LiveMonitoringDTO
             {
                 Records = g.Key.GetValueOrDefault() == false ? "Pending Records" : "Successful Records",
                 Count = g.Count()
             })
             .ToListAsync();

            return result;
        }

        public async Task<List<SignatureMonitoringDTO>> GetSignatureDataAsync()
        {
            var query = await _repository.GetAllQueryableAsync();
            query = query.Where(x => x.Date.Date == DateTime.Today &&
                                     !x.IsDeleted &&
                                     x.AccountNumber != "0000000000000000" &&
                                     !x.AccountNumber.StartsWith("00017571") &&
                                     !x.AccountNumber.StartsWith("00017574") &&
                                     !x.AccountNumber.StartsWith("000000"));

            var result = await query
      .GroupBy(x => x.serviceRun)
      .Select(g => new SignatureMonitoringDTO
      {
          Records = g.Key.GetValueOrDefault() == false ? "Pending Records" : "Successful Records",
          Count = g.Count()
      })
      .ToListAsync();

            return result;
        }
    }
}