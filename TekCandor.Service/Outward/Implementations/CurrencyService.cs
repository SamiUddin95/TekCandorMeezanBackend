using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CurrencyDTO>> GetAllAsync()
        {
            var currencies = await _repository.GetAllAsync();

            return currencies.Select(c => new CurrencyDTO
            {
                DisplayLocale = c.DisplayLocale
            }).ToList();
        }
    }
}
