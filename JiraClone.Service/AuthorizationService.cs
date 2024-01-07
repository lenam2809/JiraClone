using AutoMapper;
using JiraClone.Data.Entities;
using JiraClone.Service.Dtos.Role;
using JiraClone.Service.Dtos.User;
using JiraClone.Service.Helpers;
using JiraClone.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JiraClone.Service
{
    public class AuthorizationService
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private readonly RoleService _roleService;
        ILogger<AuthorizationService> _logger;



        public AuthorizationService(RoleService roleService, IMapper mapper, UserManager<User> userManager, ILogger<AuthorizationService> logger, IConfiguration configuration)
        {
            _roleService = roleService;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }


        //public async Task<List<string>> GetUserPrivileges(int userId)
        //{
        //    return await _repository.Filter<UserPrivilege>(x => x.UserId == userId).Select(x => x.PrivilegeId).ToListAsync();
        //}


        //public async Task SaveUserPrivileges(int userId, string[] privileges)
        //{
        //    using (var ts = _repository.BeginTransaction())
        //    {
        //        var db = _repository.GetDbContext<EPSContext>();
        //        var userPrivileges = await db.UserPrivileges.Include(x => x.Privilege).Where(x => x.UserId == userId).ToListAsync();

        //        foreach (var removingPrivilges in userPrivileges.Where(x => !privileges.Contains(x.PrivilegeId)))
        //        {
        //            db.Remove(removingPrivilges);
        //        }

        //        foreach (var newPrivilege in privileges.Where(x => !userPrivileges.Any(y => y.PrivilegeId == x)))
        //        {
        //            db.Add(new UserPrivilege() { UserId = userId, PrivilegeId = newPrivilege });
        //        }

        //        await db.SaveChangesAsync();

        //        ts.Commit();
        //    }
        //}

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
            await _roleService.AddAsync(roleCreate);
            return roleCreate.Id;
        }

        public async Task<PagingResult<RoleGridDto>> GetRoles(RoleGridPagingDto pagingModel)
        {
            return await _roleService.FilterPagedAsync<Role, RoleGridDto>(pagingModel);
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
            using (var ts = _repository.BeginTransaction())
            {
                var entityUser = _mapper.Map<User>(newUser);

                var result = await _userManager.CreateAsync(entityUser, newUser.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(".", result.Errors.Select(x => x.Description));

                    throw new Exception(errors);
                }
                else
                {
                }

                ts.Commit();
                return entityUser.Id;
            }
        }
        public async Task<int> CreateUser(UserAddDto newUser)
        {
            using (var ts = _repository.BeginTransaction())
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
            using (var ts = _repository.BeginTransaction())
            {
                await _baseService.UpdateAsync<User, UserUpdateDto>(id, editedUser);

                if (!string.IsNullOrEmpty(editedUser.NewPassword))
                {
                    var user = await _baseService.GetDbContext<Data.JiraCloneDbContext>().Users.FindAsync(id);
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var result = await _userManager.ResetPasswordAsync(user, token, editedUser.NewPassword);

                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join(".", result.Errors.Select(x => x.Description)));
                    }
                    else
                    {
                        // Must log manually if not using BaseService
                    }
                }

                ts.Commit();
                return true;
            }
        }
        public async Task<bool> UpdateInfoUser(int id, UserUpdateInfoDto oUser)
        {
            using (var ts = _repository.BeginTransaction())
            {
                await _baseService.UpdateAsync<User, UserUpdateInfoDto>(id, oUser);
                return true;
            }
        }
        #endregion

    }
}
