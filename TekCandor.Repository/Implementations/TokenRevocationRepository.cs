using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class TokenRevocationRepository : ITokenRevocationRepository
    {
        private readonly AppDbContext _context;
        public TokenRevocationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsRevokedAsync(string jti)
        {
            return await _context.RevokedTokens.AsNoTracking().AnyAsync(r => r.Jti == jti);
        }
        public async Task RevokeAsync(string jti, DateTime expiresAt)
        {
            if (await IsRevokedAsync(jti)) return;
            _context.RevokedTokens.Add(new Repository.Entities.RevokedToken
            {
               
                Jti = jti,
                RevokedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            });
            await _context.SaveChangesAsync();
        }
    }
}
