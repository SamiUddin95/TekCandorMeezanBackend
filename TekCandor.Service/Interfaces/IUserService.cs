using System;
using System.Collections.Generic;
using TekCandor.Repository.Entities;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserPagedResult<UserDTO>> GetAll(int pageNumber, int pageSize);
        UserDTO? GetById(long id);
        UserDTO Create(UserDTO user);
        UserDTO? Update(UserDTO user);
        bool SoftDelete(long id);
        Task<UserDTO?> ValidateCredentialsAsync(string loginName, string password);
        Task<UserDTO> CreateAsync(UserDTO user, string password);

    }
}
