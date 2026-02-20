using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;

        public SettingRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool UpdateCallbackAmount(string callbackAmount)
        {
            var existing = _context.Setting
                .FirstOrDefault(s => s.Name == "callbackamount" && !s.IsDeleted);

            if (existing == null)
                return false;

            existing.Value = callbackAmount;
            existing.UpdatedBy = "System";
            existing.UpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return true;
        }

    }
}
