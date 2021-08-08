using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solution.Application.Roles;
using Solution.Application.Users;
using Solution.ViewModels.Common;
using Solution.ViewModels.Roles;
using Solution.ViewModels.Users;
using System;
using System.Threading.Tasks;

namespace SolutionForBusiness.BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        public readonly IUserService _userService;
        public readonly IRoleService _roleService;

        public UsersController(IUserService userService, IRoleService roleService)
        {
            _roleService = roleService;
            _userService = userService;
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

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteRequest request)
        {
            var userCheck = await _userService.checkRoleUser(request.userId);
            if (userCheck.IsSuccessed)
            {
                var delete = await _userService.Delete(request.Id);
                return Ok(delete);
            }
            return BadRequest(userCheck.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            var user = await _userService.Update(request);
            if (user.IsSuccessed) return Ok(user.Message);
            return BadRequest("cập nhật thất bại");
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var user = await _userService.UpdatePassword(request);
            if (user.IsSuccessed) return Ok(user.Message);
            return BadRequest(user.Message);
        }

        [HttpPost("rolesassign")]
        public async Task<IActionResult> RoleAssign([FromBody] RoleAssignRequest request)
        {
            var userCheck = await _userService.checkRoleUser(request.userIdRole);
            if (userCheck.IsSuccessed)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _userService.RoleAssign(request);
                if (result.IsSuccessed) return Ok("Cập nhật thành Công");
            }

            return BadRequest();
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