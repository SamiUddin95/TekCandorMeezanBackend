using System;
using System.Collections.Generic;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IReturnReasonService
    {
        Task<PagedResult<ReturnReasonDTO>> GetAll(int pageNumber, int pageSize, string? name = null);
        ReturnReasonDTO? GetById(long id);
        ReturnReasonDTO Create(ReturnReasonDTO returnReason);
        ReturnReasonDTO? Update(ReturnReasonDTO returnReason);
        bool SoftDelete(long id);
    }
}
