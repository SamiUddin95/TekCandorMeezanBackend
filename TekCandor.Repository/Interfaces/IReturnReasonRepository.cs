using System;
using System.Collections.Generic;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IReturnReasonRepository
    {
        IEnumerable<ReturnReason> GetAll();
        ReturnReason? GetById(Guid id);
        ReturnReason Add(ReturnReason returnReason);
        ReturnReason? Update(ReturnReason returnReason);
        bool SoftDelete(Guid id);
    }
}
