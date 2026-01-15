using System;
using System.Collections.Generic;
using System.Linq;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class HubRepository : IHubRepository
    {
        private readonly AppDbContext _context;

        public HubRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Hub> GetAll()
        {
            return _context.Hub.ToList();
        }

        public Hub Add(Hub hub)
        {
            hub.Id = Guid.NewGuid();
            hub.CreatedDateTime = DateTime.UtcNow;
            _context.Hub.Add(hub);
            _context.SaveChanges();
            return hub;
        }

        public Hub? GetById(Guid id)
        {
            return _context.Hub.FirstOrDefault(h => h.Id == id);
        }

        public Hub? Update(Hub hub)
        {
            var existing = _context.Hub.FirstOrDefault(h => h.Id == hub.Id);
            if (existing == null) return null;

            existing.Code = hub.Code;
            existing.Name = hub.Name;
            existing.IsDeleted = hub.IsDeleted;
            existing.IsNew = hub.IsNew;
            existing.Version = hub.Version;
            existing.ModifiedUser = hub.ModifiedUser;
            existing.ModifiesDateTime = DateTime.UtcNow;
            existing.CrAccSameDay = hub.CrAccSameDay;
            existing.CrAccNormal = hub.CrAccNormal;
            existing.CrAccIntercity = hub.CrAccIntercity;
            existing.CrAccDollar = hub.CrAccDollar;

            _context.SaveChanges();
            return existing;
        }

        public bool SoftDelete(Guid id)
        {
            var existing = _context.Hub.FirstOrDefault(h => h.Id == id);
            if (existing == null) return false;
            if (existing.IsDeleted) return true;
            existing.IsDeleted = true;
            existing.ModifiesDateTime = DateTime.UtcNow;
            _context.SaveChanges();
            return true;
        }
    }
}
