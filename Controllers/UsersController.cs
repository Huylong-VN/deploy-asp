using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Solution.Application.Roles;
using Solution.Application.Users;
using Solution.Controllers;
using Solution.ViewModels.Common;
using Solution.ViewModels.Roles;
using Solution.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SolutionForBusiness.BackEndApi.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, IRoleService roleService, IConfiguration configuration)
        {
            _configuration = configuration;
            _roleService = roleService;
            _userService = userService;
        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] GetRefreshTokenRequest refreshToken)
        {
            var result = await _userService.GetRefreshToken(refreshToken);
            if (result != null) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);

            var result = await _userService.Authenticate(request);

            if (result.ResultObj == null) return BadRequest(result.Message);
            return Ok(result.ResultObj);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromHeader] GetUserPagingRequest request)
        {
            var result = await _userService.GetUserPaging(request);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (result.IsSuccessed) return Ok(result.Message);
            return BadRequest(result.Message);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var delete = await _userService.Delete(Id);
            if (delete.IsSuccessed) return Ok(delete);
            return BadRequest(delete.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            var user = await _userService.Update(request);
            if (user.IsSuccessed) return Ok(user.ResultObj);
            return BadRequest(user.Message);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var user = await _userService.UpdatePassword(request);
            if (user.IsSuccessed) return Ok(user.Message);
            return BadRequest(user.Message);
        }

        [HttpPut("resetpassword/{Id}")]
        public async Task<IActionResult> ResetPassword(Guid Id, string newPassword)
        {
            var result = await _userService.ResetPassword(Id, newPassword);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("rolesassign")]
        public async Task<IActionResult> RoleAssign([FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.RoleAssign(request);
            if (result.IsSuccessed) return Ok("Cập nhật thành Công");
            return BadRequest(result.Message);
        }

        [HttpGet("getrole/{id}")]
        public async Task<IActionResult> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userService.getById(id);
            var roleObj = await _roleService.getAll();
            var roleAssignRequest = new RoleAssignRequest() { };
            foreach (var role in roleObj)
            {
                roleAssignRequest.Roles.Add(new SelectedItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return Ok(roleAssignRequest);
        }
    }
}