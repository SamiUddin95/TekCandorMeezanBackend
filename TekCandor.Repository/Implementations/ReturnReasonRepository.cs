using System;
using System.Collections.Generic;
using System.Linq;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ReturnReasonRepository : IReturnReasonRepository
    {
        private readonly AppDbContext _context;
        public ReturnReasonRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ReturnReason> GetAll()
        {
            return _context.ReturnReason;
        }

        public ReturnReason? GetById(Guid id)
        {
            return _context.ReturnReason.FirstOrDefault(r => r.Id == id);
        }

        public ReturnReason Add(ReturnReason returnReason)
        {
            returnReason.Id = Guid.NewGuid();
            returnReason.CreatedDateTime = DateTime.Now;
            _context.ReturnReason.Add(returnReason);
            _context.SaveChanges();
            return returnReason;
        }

        public ReturnReason? Update(ReturnReason returnReason)
        {
            var existing = _context.ReturnReason.FirstOrDefault(r => r.Id == returnReason.Id);
            if (existing == null) return null;

            existing.Code = returnReason.Code;
            existing.AlphaReturnCodes = returnReason.AlphaReturnCodes;
            existing.NumericReturnCodes = returnReason.NumericReturnCodes;
            existing.DescriptionWithReturnCodes = returnReason.DescriptionWithReturnCodes;
            existing.DefaultCallBack = returnReason.DefaultCallBack;
            existing.Version = returnReason.Version;
            existing.Name = returnReason.Name;
            existing.IsNew = returnReason.IsNew;
            existing.IsDeleted = returnReason.IsDeleted;
            existing.CreatedUser = returnReason.CreatedUser;
            existing.CreatedDateTime = returnReason.CreatedDateTime;
            existing.ModifiedUser = returnReason.ModifiedUser;
            existing.ModifiedDateTime = DateTime.Now;

            _context.SaveChanges();
            return existing;
        }

        public bool SoftDelete(Guid id)
        {
            var existing = _context.ReturnReason.FirstOrDefault(r => r.Id == id);
            if (existing == null) return false;
            if (existing.IsDeleted) return true;

            existing.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }
    }
}
