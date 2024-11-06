using AutoMapper;
using BLL.Shared;
using DAL.Abstraction;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly UserManager<User> _userManager;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, IDistributedCache distributedCache, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _userManager = userManager;
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllUsersAsync(string searchTerm = null, string sortBy = "UserName", bool ascending = true, int page = 1, int pageSize = 10)
        {
            string cacheKey = $"Users_{searchTerm}_{sortBy}_{ascending}_{page}_{pageSize}";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<GetUserDTO> cachedUsers))
            {
                return cachedUsers;
            }

            var users = await _unitOfWork.Users.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                users = users.Where(u => u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                          u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            users = sortBy switch
            {
                "Email" => ascending ? users.OrderBy(u => u.Email) : users.OrderByDescending(u => u.Email),
                _ => ascending ? users.OrderBy(u => u.UserName) : users.OrderByDescending(u => u.UserName),
            };

            var result = users.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .Select(user => _mapper.Map<GetUserDTO>(user))
                              .ToList();


            _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                Size = 1
            });

            return result;
        }

        public async Task<GetUserDTO> GetUserByIdAsync(Guid id)
        {
            string cacheKey = $"User_{id}";
            var cachedUser = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedUser != null)
            {
                return JsonSerializer.Deserialize<GetUserDTO>(cachedUser);
            }

            if (_memoryCache.TryGetValue(cacheKey, out GetUserDTO user))
            {
                return user;
            }

            var userEntity = await _unitOfWork.Users.GetByIdAsync(id);
            if (userEntity == null)
            {
                return null;
            }

            user = _mapper.Map<GetUserDTO>(userEntity);


            _memoryCache.Set(cacheKey, user, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                Size = 1
            });


            var serializedUser = JsonSerializer.Serialize(user);
            await _distributedCache.SetStringAsync(cacheKey, serializedUser, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return user;
        }

        public async Task<IdentityResult> AddUserAsync(CreateUserDTO userDto, string roleName)
        {
            var user = _mapper.Map<User>(userDto);


            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                return result;
            }


            if (!string.IsNullOrEmpty(roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return result;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDTO userDto, string userNameToUpdate)
        {
            var user = await _userManager.FindByNameAsync(userNameToUpdate);
            if (user == null)
                return false;
            user.Email = userDto.Email;
            user.UserName = userDto.UserName;


            //await _unitOfWork.Users.UpdateAsync(user);
            //await _unitOfWork.CompleteAsync();
            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {

                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                    return false;


                var addPasswordResult = await _userManager.AddPasswordAsync(user, userDto.Password);
                if (!addPasswordResult.Succeeded)
                    return false;
            }


            await _userManager.UpdateSecurityStampAsync(user);


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return false;


            string cacheKey = $"User_{user.Id}";
            _memoryCache.Remove(cacheKey);
            await _distributedCache.RemoveAsync(cacheKey);

            return true;
        }


        public async Task DeleteUserAsync(Guid id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();


            string cacheKey = $"User_{id}";
            _memoryCache.Remove(cacheKey);
            await _distributedCache.RemoveAsync(cacheKey);
        }

        public async Task<bool> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
    }
}

