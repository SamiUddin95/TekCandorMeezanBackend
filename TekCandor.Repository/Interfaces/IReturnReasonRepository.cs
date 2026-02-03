using System;
using System.Collections.Generic;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IReturnReasonRepository
    {
        IQueryable<ReturnReason> GetAllQueryable();
        ReturnReason? GetById(long id);
        ReturnReason Add(ReturnReason returnReason);
        ReturnReason? Update(ReturnReason returnReason);
        bool SoftDelete(long id);
    }
}
