using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IHubRepository
    {
        IEnumerable<Hub> GetAll();
        Hub Add(Hub hub);
        Hub? GetById(Guid id);
        Hub? Update(Hub hub);
        bool SoftDelete(Guid id);
    }
}
