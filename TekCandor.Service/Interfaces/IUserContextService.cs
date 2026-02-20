using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Interfaces
{
    public record UserScope(string BranchOrHub, string? HubIds, string? BranchCodes);

    public interface IUserContextService
    {
        Task<UserScope> GetUserScopeAsync(long userId, CancellationToken cancellationToken = default);
    }
}
