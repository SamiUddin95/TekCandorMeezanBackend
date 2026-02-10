using Microsoft.EntityFrameworkCore;
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
        public async Task<IQueryable<ReturnReason>> GetAllQueryableAsync()
        {
            return _context.ReturnReason.AsNoTracking();
        }

        //public IQueryable<ReturnReason> GetAllQueryable()
        //{
        //    return _context.ReturnReason.AsNoTracking();
        //}


        public ReturnReason? GetById(long id)
        {
            return _context.ReturnReason.FirstOrDefault(r => r.Id == id);
        }

        public ReturnReason Add(ReturnReason entity)
        {
            entity.CreatedOn = DateTime.Now; 
            _context.ReturnReason.Add(entity);
            _context.SaveChanges();
            return entity;
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
            existing.Name = returnReason.Name;
            existing.IsDeleted = returnReason.IsDeleted;
            existing.CreatedBy = returnReason.CreatedBy;
            existing.CreatedOn = returnReason.CreatedOn;
            existing.UpdatedBy = returnReason.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;

            _context.SaveChanges();
            return existing;
        }

        public bool SoftDelete(long id)
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
