using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllQueryable()
        {
            return _context.Users
                   .AsNoTracking();           
        }

        public User? GetById(long id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Add(User user)
        {
           
            user.CreatedOn = DateTime.Now;
            user.IsActive = true;

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User? Update(User user)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null) return null;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.PhoneNo = user.PhoneNo;
            existing.LoginName = user.LoginName;
            existing.PasswordHash = user.PasswordHash;
            existing.IsActive = user.IsActive;
            existing.IsSupervise = user.IsSupervise;
            existing.UpdatedBy = user.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;
            existing.UserType = user.UserType;

            _context.SaveChanges();
            return existing;
        }

        
        public bool SoftDelete(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedOn = DateTime.Now;

            _context.SaveChanges();
            return true;
        }
        public async Task<(long Id, string PasswordHash, bool IsActive)?> GetAuthByLoginAsync(string loginName)
        {
            var data = await _context.Users.AsNoTracking()
                .Where(u => u.LoginName == loginName)
                .Select(u => new { u.Id, u.PasswordHash, u.IsActive, u.IsSupervise })
                .FirstOrDefaultAsync();
            if (data == null) return null;
            return (data.Id, data.PasswordHash!, data.IsActive);
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNo = u.PhoneNo,
                    LoginName = u.LoginName,
                    IsActive = u.IsActive,
                    CreatedBy = u.CreatedBy,
                    CreatedOn = u.CreatedOn,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedOn = u.UpdatedOn,
                    PasswordHash = u.PasswordHash,
                    IsSupervise = u.IsSupervise,
                    UserType = u.UserType
                }).FirstOrDefaultAsync();
        }
       
        public async Task<User> AddAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


    }
}
