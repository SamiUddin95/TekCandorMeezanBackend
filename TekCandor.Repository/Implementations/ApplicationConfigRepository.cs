using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ApplicationConfigRepository : IApplicationConfigRepository
    {
        private readonly AppDbContext _context;

        public ApplicationConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetValueByKeyAsync(string key)
        {
            var config = await _context.ApplicationConfig
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Key == key && !c.IsDeleted);

            return config?.Value;
        }

        public async Task<bool> UpdateValueByKeyAsync(string key, string value, string updatedBy)
        {
            var config = await _context.ApplicationConfig
                .FirstOrDefaultAsync(c => c.Key == key && !c.IsDeleted);

            if (config == null)
            {
                _context.ApplicationConfig.Add(new Entities.ApplicationConfig
                {
                    Key = key,
                    Value = value,
                    Name = key,
                    IsDeleted = false,
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                });
            }
            else
            {
                config.Value = value;
                config.UpdatedBy = updatedBy;
                config.UpdatedOn = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
