using System;
using System.Collections.Generic;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IReturnReasonService
    {
        IEnumerable<ReturnReasonDTO> GetAll();
        ReturnReasonDTO? GetById(long id);
        ReturnReasonDTO Create(ReturnReasonDTO returnReason);
        ReturnReasonDTO? Update(ReturnReasonDTO returnReason);
        bool SoftDelete(long id);
    }
}
