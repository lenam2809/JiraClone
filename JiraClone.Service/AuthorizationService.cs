using AutoMapper;
using JiraClone.Data;
using JiraClone.Data.Entities;
using JiraClone.Service.Dtos.Role;
using JiraClone.Service.Dtos.User;
using JiraClone.Service.Helpers;
using JiraClone.Utils.BaseService;
using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices.JavaScript;

namespace JiraClone.Service
{
    public class AuthorizationService
    {
        private BaseService _baseService;

        private IMapper _mapper;
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        ILogger<AuthorizationService> _logger;



        public AuthorizationService(BaseService baseService, IMapper mapper, UserManager<User> userManager, ILogger<AuthorizationService> logger, IConfiguration configuration)
        {
            _baseService = baseService;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> ChangePassword(string userName, ChangePasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new Exception(string.Format(". \n\r", result.Errors.Select(x => x.Description)));
            }
        }
        public async Task<bool> ResetPassword(string userName, string OldPassword, string NewPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            // var result = await _userManager.ResetPasswordAsync(user, code, NewPassword);
            var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new Exception(string.Format(". \n\r", result.Errors.Select(x => x.Description)));
            }
        }

        #region Role
        public async Task<int> CreateRole(RoleCreateDto roleCreate)
        {
            await _baseService.CreateAsync<Role, RoleCreateDto>(roleCreate);
            return roleCreate.Id;
        }

        public async Task<PagingResult<RoleGridDto>> GetRoles(RoleGridPagingDto pagingModel)
        {
            return await _baseService.FilterPagedAsync<Role, RoleGridDto>(pagingModel);
        }

        public async Task<int> DeleteRole(int id)
        {
            return await _baseService.DeleteAsync<Role, int>(id);
        }

        public async Task<int> DeleteRole(int[] ids)
        {
            return await _baseService.DeleteAsync<Role, int>(ids);
        }

        public async Task<RoleDetailDto> GetRoleById(int id)
        {
            return await _baseService.FindAsync<Role, RoleDetailDto>(id);
        }

        public async Task<int> UpdateRole(int id, RoleUpdateDto editedRole)
        {
            return await _baseService.UpdateAsync<Role, RoleUpdateDto>(id, editedRole);
        }
        #endregion

        #region User
        public async Task<int> CreateUser(UserCreateDto newUser)
        {
            using (var ts = _baseService.BeginTransaction())
            {
                var entityUser = _mapper.Map<User>(newUser);

                var result = await _userManager.CreateAsync(entityUser, newUser.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(".", result.Errors.Select(x => x.Description));

                    throw new JiraCloneException(errors);
                }

                ts.Commit();
                return entityUser.Id;
            }
        }
        public async Task<int> CreateUser(UserAddDto newUser)
        {
            using (var ts = _baseService.BeginTransaction())
            {
                var entityUser = _mapper.Map<User>(newUser);

                var result = await _userManager.CreateAsync(entityUser, newUser.Password);
                ts.Commit();
                return newUser.Id;
            }
        }

        public async Task<PagingResult<UserGridDto>> GetUsers(UserGridPagingDto pagingModel)
        {
            return await _baseService.FilterPagedAsync<User, UserGridDto>(pagingModel);
        }

        public async Task<int> DeleteUser(int id)
        {
            return await _baseService.DeleteAsync<User, int>(id);
        }

        public async Task<int> DeleteUser(int[] ids)
        {
            return await _baseService.DeleteAsync<User, int>(ids);
        }

        public async Task<UserDetailDto> GetUserById(int id)
        {
            return await _baseService.FindAsync<User, UserDetailDto>(id);
        }

        public async Task<UserDetailDto> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return await _baseService.FindAsync<User, UserDetailDto>(user.Id);
            else
                return new UserDetailDto();
        }

        public async Task<bool> UpdateUser(int id, UserUpdateDto editedUser)
        {
            using (var ts = _baseService.BeginTransaction())
            {
                await _baseService.UpdateAsync<User, UserUpdateDto>(id, editedUser);

                //await SaveUserRoles(id, editedUser.RoleIds);
                if (!string.IsNullOrEmpty(editedUser.NewPassword))
                {
                    var user = await _baseService.GetDbContext<JiraCloneDbContext>().Users.FindAsync(id);
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var result = await _userManager.ResetPasswordAsync(user, token, editedUser.NewPassword);

                    if (!result.Succeeded)
                    {
                        throw new JiraCloneException(string.Join(".", result.Errors.Select(x => x.Description)));
                    }
                }

                ts.Commit();
                return true;
            }
        }
        public async Task<bool> UpdateInfoUser(int id, UserUpdateInfoDto oUser)
        {
            using (var ts = _baseService.BeginTransaction())
            {
                await _baseService.UpdateAsync<User, UserUpdateInfoDto>(id, oUser);
                return true;
            }
        }
        #endregion

    }
}
