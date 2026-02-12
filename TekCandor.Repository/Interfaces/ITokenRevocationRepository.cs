using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Interfaces
{
    public interface ITokenRevocationRepository
    {
        Task<bool> IsRevokedAsync(string jti);
        Task RevokeAsync(string jti, DateTime expiresAt);
    }
}
