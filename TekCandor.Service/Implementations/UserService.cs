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


        public async Task<PagedResult<UserDTO>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _repository.GetAllQueryable()
                        .Where(u => u.IsActive);  

            int totalUsers = await _repository.GetAllQueryable().CountAsync();

            int totalActiveUsers = await query.Where(u => u.IsActive == true).CountAsync();

            int totalHubUser = await query
                .Where(u => u.BranchorHub == "Hub")  
                .CountAsync();

            int totalBranchUser = await query
                .Where(u => u.BranchorHub == "Branch")  
                .CountAsync();


            var users = await query
                .OrderByDescending(u => u.UpdatedOn ?? u.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                LoginName = u.LoginName,
                Email = u.Email,
                BranchorHub = u.BranchorHub,
                HubIds = u.HubIds,
                BranchIds = u.BranchIds,
                GroupId = u.GroupId,
                PhoneNo = u.PhoneNo,
                IsActive = u.IsActive,
                CreatedBy = u.CreatedBy,
                CreatedOn = u.CreatedOn,
                UpdatedBy = u.UpdatedBy,
                UpdatedOn = u.UpdatedOn,
                UserLimit = u.UserLimit,
               

            });

            return new PagedResult<UserDTO>
            {
                Items = dtos,
                TotalCount = totalUsers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalUsers = totalUsers,
                TotalHubUser = totalHubUser,
                TotalActiveUser = totalActiveUsers,
                TotalBranchUser = totalBranchUser

            };
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
               
                HubIds = u.HubIds,
                BranchIds = u.BranchIds,
                GroupId=u.GroupId,
                PhoneNo = u.PhoneNo,
             
                IsActive = u.IsActive,
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
               
                HubIds = dto.HubIds,
                BranchIds = dto.BranchIds,
                GroupId=dto.GroupId,
                PhoneNo = dto.PhoneNo,
                //PasswordLastChanged= dto.PasswordLastChanged,
                //LastLoginTime= dto.LastLoginTime,
                IsActive = true,
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
               
                HubIds = dto.HubIds,
                BranchIds = dto.BranchIds,
                GroupId=dto.GroupId,
                PhoneNo = dto.PhoneNo,
              
                IsActive = dto.IsActive,
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
               
                HubIds = user.HubIds,
                BranchIds = user.BranchIds,
                GroupId=user.GroupId,
                PhoneNo = user.PhoneNo,
                //PasswordLastChanged = user.PasswordLastChanged,
                //LastLoginTime = user.LastLoginTime,
                IsActive = true,
                CreatedBy = user.CreatedBy ?? "system",
                CreatedOn = DateTime.Now,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = DateTime.Now,
                UserLimit= user.UserLimit
            };
        }


        //public async Task<UserDTO> CreateAsync(UserDTO dto, string password)
        //{
        //    var userEntity = new User
        //    {
        //        Id = dto.Id,
        //        Name = dto.Name,
        //        Email = dto.Email,
        //        PhoneNo = dto.PhoneNo,
        //        LoginName=dto.LoginName,
        //        PasswordHash=dto.PasswordHash,
        //        BranchorHub=dto.BranchorHub,

        //        HubIds=dto.HubIds,
        //        BranchIds=dto.BranchIds,
        //        GroupId=dto.GroupId,
        //        IsActive=dto.IsActive,
        //        CreatedBy=dto.CreatedBy,
        //        CreatedOn=dto.CreatedOn,
        //        UpdatedBy=dto.UpdatedBy,
        //        UpdatedOn=dto.UpdatedOn,
        //        UserLimit=dto.UserLimit

        //    };

        //    var hash = HashPassword(password);

        //    var createdUser = await _repository.AddAsync(userEntity, hash);


        //    return new UserDTO
        //    {
        //        Id = createdUser.Id,
        //        Name = createdUser.Name,
        //        Email = createdUser.Email,
        //        PhoneNo = createdUser.PhoneNo,
        //        LoginName=createdUser.LoginName,
        //        PasswordHash = createdUser.PasswordHash,
        //        BranchorHub = createdUser.BranchorHub,

        //        HubIds = createdUser.HubIds,
        //        BranchIds = createdUser.BranchIds,
        //        GroupId=createdUser.GroupId,
        //        IsActive = createdUser.IsActive,
        //        CreatedBy = createdUser.CreatedBy,
        //        CreatedOn = createdUser.CreatedOn,
        //        UpdatedBy = createdUser.UpdatedBy,
        //        UpdatedOn = createdUser.UpdatedOn,
        //        UserLimit = createdUser.UserLimit


        //    };
        //}
        public async Task<UserDTO> CreateAsync(UserDTO dto, string password)
        {
            var userEntity = new User
            {
                
                Name = dto.Name,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                LoginName = dto.LoginName,
                BranchorHub = dto.BranchorHub,
                HubIds = dto.HubIds,
                BranchIds = dto.BranchIds,
                GroupId = dto.GroupId,
                IsActive = dto.IsActive,
                UserLimit = dto.UserLimit,

                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.Now,

                UpdatedBy = null,
                UpdatedOn = null
            };

            var hash = HashPassword(password);

            var createdUser = await _repository.AddAsync(userEntity, hash);

            return new UserDTO
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                PhoneNo = createdUser.PhoneNo,
                LoginName = createdUser.LoginName,
                PasswordHash = createdUser.PasswordHash,
                BranchorHub = createdUser.BranchorHub,
                HubIds = createdUser.HubIds,
                BranchIds = createdUser.BranchIds,
                GroupId = createdUser.GroupId,
                IsActive = createdUser.IsActive,
                CreatedBy = createdUser.CreatedBy,
                CreatedOn = createdUser.CreatedOn,
                UpdatedBy = createdUser.UpdatedBy,   
                UpdatedOn = createdUser.UpdatedOn,   
                UserLimit = createdUser.UserLimit
            };
        }

    }
}
