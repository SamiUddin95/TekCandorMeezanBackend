using System;
using System.Collections.Generic;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllQueryable();
        User? GetById(long id);
        User Add(User user);
        User? Update(User user);
        bool SoftDelete(long id);
        Task<(long Id, string PasswordHash, bool IsActive)?> GetAuthByLoginAsync(string loginName);
        Task<User?> GetByIdAsync(long id);

        Task<User> AddAsync(User user, string passwordHash);

        Task<List<string>> GetUserPermissionsAsync(long userId);


    }
}
