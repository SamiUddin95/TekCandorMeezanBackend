using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class CycleRepository : ICycleRepository
    {
        private readonly AppDbContext _context;

        public CycleRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cycle> GetAllQueryable()
        {
            return _context.Cycles.AsNoTracking();
        }


        public Cycle Add(Cycle cycle)
        {
            
            cycle.CreatedOn = DateTime.Now;
            _context.Cycles.Add(cycle);
            _context.SaveChanges();
            return cycle;
        }

        public Cycle? GetById(long id)
        {
            return _context.Cycles.FirstOrDefault(c => c.Id == id);
        }

        public Cycle? Update(Cycle cycle)
        {
            var existing = _context.Cycles.FirstOrDefault(c => c.Id == cycle.Id);
            if (existing == null) return null;

            existing.Code = cycle.Code;
            existing.Name = cycle.Name;
            existing.IsDeleted = cycle.IsDeleted;
            existing.UpdatedBy = cycle.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;

            _context.SaveChanges();
            return existing;
        }

        public bool SoftDelete(long id)
        {
            var existing = _context.Cycles.FirstOrDefault(c => c.Id == id);
            if (existing == null) return false;
            if (existing.IsDeleted) return true;
            existing.IsDeleted = true;
            existing.UpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
    }
}
