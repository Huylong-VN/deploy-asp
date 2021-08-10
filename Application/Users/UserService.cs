using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Solution.Data.Entities;
using Solution.ViewModels.Common;
using Solution.ViewModels.Roles;
using Solution.ViewModels.Users;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Application.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _usermanage;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _configuration = configuration;
            _usermanage = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResult<UserVM>> Authenticate(LoginRequest request)
        {
            var user = await _usermanage.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<UserVM>("Tài khoản không tồn tại");
            var role = await _usermanage.GetRolesAsync(user);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (!result.Succeeded) return new ApiErrorResult<UserVM>("Đăng Nhập thất bại");
            var roles = await _usermanage.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"], claims, expires: DateTime.Now.AddHours(3), signingCredentials: creds);

            return new ApiSuccessResult<UserVM>(new UserVM
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                LastName = user.LastName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = role,
            });
        }

        public async Task<ApiResult<bool>> Delete(Guid Id)
        {
            var user = await _usermanage.FindByIdAsync(Id.ToString());
            if (user == null) return new ApiErrorResult<bool>("Người dùng không tồn tại");
            await _usermanage.DeleteAsync(user);
            return new ApiSuccessResult<bool>();
        }

        public async Task<PagedResult<UserVM>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = _usermanage.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.Email.Contains(request.Keyword));
            }

            //Total row
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).OrderBy(x => x.FirstName).Take(request.PageSize).Select(x => new UserVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Dob = x.Dob,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName
            }).OrderBy(x => x.Id).ToListAsync();

            var pagedResult = new PagedResult<UserVM>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ToTalRecords = totalRow
            };

            return pagedResult;
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            if (await _usermanage.FindByNameAsync(request.UserName) != null) return new ApiErrorResult<bool>("Tên tài khoản đã tồn tại");
            if (await _usermanage.FindByEmailAsync(request.Email) != null) return new ApiErrorResult<bool>("Email đã tồn tại");
            var user = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _usermanage.CreateAsync(user, request.Password);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Đăng kí thất bại");
        }

        public async Task<ApiResult<bool>> RoleAssign(RoleAssignRequest request)
        {
            var user = await _usermanage.FindByIdAsync(request.userId.ToString());
            if (user == null) return new ApiErrorResult<bool>("Tài khoản không tồn tại");

            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();

            foreach (var roleName in removedRoles)
            {
                if (await _usermanage.IsInRoleAsync(user, roleName) == true)
                {
                    await _usermanage.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _usermanage.RemoveFromRolesAsync(user, removedRoles);

            var addRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var rolename in addRoles)
            {
                var check = await _usermanage.IsInRoleAsync(user, rolename);
                if (check == false)
                {
                    await _usermanage.AddToRoleAsync(user, rolename);
                }
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<Guid>> Update(UpdateRequest request)
        {
            if (await _usermanage.Users.AnyAsync(x => x.Email == request.Email && x.Id != request.Id)) return new ApiErrorResult<Guid>("Email đã tồn tại");
            var user = await _usermanage.FindByIdAsync(request.Id.ToString());
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Dob = request.Dob;
            user.PhoneNumber = request.Phone;
            var result = await _usermanage.UpdateAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<Guid>(request.Id);
            return new ApiErrorResult<Guid>("Update thất bại");
        }

        public async Task<ApiResult<bool>> checkRoleUser(Guid userId)
        {
            var userRole = await _usermanage.FindByIdAsync(userId.ToString());
            var role = await _usermanage.GetRolesAsync(userRole);
            bool check = false;
            foreach (var r in role)
            {
                if (r.Contains("Administrator"))
                {
                    check = true; break;
                }
            }
            if (check == true) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Bạn chưa có quyền");
        }

        public async Task<ApiResult<bool>> UpdatePassword(UpdatePasswordRequest request)
        {
            var user = await _usermanage.FindByIdAsync(request.Id.ToString());

            if (user.PasswordHash == request.NewPassword) return new ApiErrorResult<bool>("Mật khẩu phải khác mật khẩu hiện tại");
            var result = await _usermanage.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Thay đổi thất bại");
        }

        public async Task<ApiResult<UserVM>> getById(Guid Id)
        {
            var user = await _usermanage.FindByIdAsync(Id.ToString());
            if (user == null) return new ApiErrorResult<UserVM>("User không tồn tại");
            var roles = await _usermanage.GetRolesAsync(user);
            var userVm = new UserVM()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Dob = user.Dob,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles
            };
            return new ApiSuccessResult<UserVM>(userVm);
        }
    }
}