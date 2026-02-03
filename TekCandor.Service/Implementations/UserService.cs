using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;


namespace TekCandor.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return _repository.GetAll().Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                LoginName = u.LoginName,
                PasswordHash=u.PasswordHash,
                Email = u.Email,
                BranchorHub= u.BranchorHub,
                UserType = u.UserType,
                HubIds = u.HubIds,
                BranchIds = u.BranchIds,
                PhoneNo = u.PhoneNo,
                PasswordLastChanged= u.PasswordLastChanged,
                LastLoginTime = u.LastLoginTime,
                IsActive = u.IsActive,
                IsSupervise = u.IsSupervise,
                CreatedBy = u.CreatedBy,
                CreatedOn = u.CreatedOn,
                UpdatedBy = u.UpdatedBy,
                UpdatedOn = u.UpdatedOn,
                UserLimit= u.UserLimit


            });
        }

        public UserDTO? GetById(long id)
        {
            var u = _repository.GetById(id);
            if (u == null) return null;

            return new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                LoginName = u.LoginName,
                PasswordHash=u.PasswordHash,
                Email = u.Email,
                BranchorHub= u.BranchorHub,
                UserType = u.UserType,
                HubIds = u.HubIds,
                BranchIds = u.BranchIds,
                PhoneNo = u.PhoneNo,
                PasswordLastChanged= u.PasswordLastChanged,
                LastLoginTime = u.LastLoginTime,
                IsActive = u.IsActive,
                IsSupervise = u.IsSupervise,
                CreatedBy = u.CreatedBy,
                CreatedOn = u.CreatedOn,
                UpdatedBy = u.UpdatedBy,
                UpdatedOn = u.UpdatedOn,
                UserLimit= u.UserLimit

            };
        }

        public UserDTO Create(UserDTO dto)
        {
            var passwordHash = HashPassword(dto.PasswordHash); 

            var entity = new User
            {
                Id=dto.Id,
                Name = dto.Name,
                LoginName = dto.LoginName,
                PasswordHash = passwordHash,
                Email = dto.Email,
                BranchorHub= dto.BranchorHub,
                UserType = dto.UserType,
                HubIds = dto.HubIds,
                BranchIds = dto.BranchIds,
                PhoneNo = dto.PhoneNo,
                PasswordLastChanged= dto.PasswordLastChanged,
                LastLoginTime= dto.LastLoginTime,
                IsActive = true,
                IsSupervise = dto.IsSupervise,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.Now,
                UpdatedBy = dto.UpdatedBy,
                UpdatedOn = dto.UpdatedOn,
                UserLimit= dto.UserLimit

            };

            var created = _repository.Add(entity);

            dto.Id = created.Id;
            dto.CreatedOn = created.CreatedOn;
            dto.PasswordHash = null; 

            return dto;
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
        public UserDTO? Update(UserDTO dto)
        {
            var entity = new User
            {
                Id = dto.Id,
                Name = dto.Name,
                LoginName = dto.LoginName,
                PasswordHash = dto.PasswordHash,
                Email = dto.Email,
                BranchorHub = dto.BranchorHub,
                UserType = dto.UserType,
                HubIds = dto.HubIds,
                BranchIds = dto.BranchIds,
                PhoneNo = dto.PhoneNo,
                PasswordLastChanged = dto.PasswordLastChanged,
                LastLoginTime = dto.LastLoginTime,
                IsActive = true,
                IsSupervise = dto.IsSupervise,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.Now,
                UpdatedBy = dto.UpdatedBy,
                UpdatedOn = dto.UpdatedOn,
                UserLimit = dto.UserLimit
            };

            var updated = _repository.Update(entity);
            if (updated == null) return null;

            return dto;
        }

        public bool SoftDelete(long id)
        {
            return _repository.SoftDelete(id);
        }

        
        public async Task<UserDTO?> ValidateCredentialsAsync(string loginName, string password)
        {
            var auth = await _repository.GetAuthByLoginAsync(loginName);
            if (auth == null) return null;

            var hash = HashPassword(password);
            if (!string.Equals(auth.Value.PasswordHash, hash, StringComparison.Ordinal))
                return null;

            var user = await _repository.GetByIdAsync(auth.Value.Id);
            if (user == null) return null;

           
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                LoginName = user.LoginName,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                BranchorHub = user.BranchorHub,
                UserType = user.UserType,
                HubIds = user.HubIds,
                BranchIds = user.BranchIds,
                PhoneNo = user.PhoneNo,
                PasswordLastChanged = user.PasswordLastChanged,
                LastLoginTime = user.LastLoginTime,
                IsActive = true,
                IsSupervise = user.IsSupervise,
                CreatedBy = user.CreatedBy ?? "system",
                CreatedOn = DateTime.Now,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = DateTime.Now,
                UserLimit= user.UserLimit
            };
        }

        
        public async Task<UserDTO> CreateAsync(UserDTO dto, string password)
        {
            var userEntity = new User
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                LoginName=dto.LoginName,
                PasswordHash=dto.PasswordHash,
                BranchorHub=dto.BranchorHub,
                UserType=dto.UserType,
                HubIds=dto.HubIds,
                BranchIds=dto.BranchIds,
                PasswordLastChanged=dto.PasswordLastChanged,
                LastLoginTime=dto.LastLoginTime,
                IsActive=dto.IsActive,
                IsSupervise=dto.IsSupervise,
                CreatedBy=dto.CreatedBy,
                CreatedOn=dto.CreatedOn,
                UpdatedBy=dto.UpdatedBy,
                UpdatedOn=dto.UpdatedOn,
                UserLimit=dto.UserLimit

            };

            var hash = HashPassword(password);

            var createdUser = await _repository.AddAsync(userEntity, hash);
           

            return new UserDTO
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                PhoneNo = createdUser.PhoneNo,
                LoginName=createdUser.LoginName,
                PasswordHash = createdUser.PasswordHash,
                BranchorHub = createdUser.BranchorHub,
                UserType = createdUser.UserType,
                HubIds = createdUser.HubIds,
                BranchIds = createdUser.BranchIds,
                PasswordLastChanged = createdUser.PasswordLastChanged,
                LastLoginTime = createdUser.LastLoginTime,
                IsActive = createdUser.IsActive,
                IsSupervise = createdUser.IsSupervise,
                CreatedBy = createdUser.CreatedBy,
                CreatedOn = createdUser.CreatedOn,
                UpdatedBy = createdUser.UpdatedBy,
                UpdatedOn = createdUser.UpdatedOn,
                UserLimit = createdUser.UserLimit


            };
        }

    }
}
