using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Solution.Application.Roles;
using System.Linq;
using System.Security.Claims;

namespace Solution.Authorization
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;
        private readonly IRoleService _roleService;

        public ClaimRequirementFilter(Claim claim, IRoleService roleService)
        {
            _roleService = roleService;
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roles = (context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "admin"));
            if (roles != null)
            {
                var listRole = roles.Value.Split(";");
                // Check xem role dods cos chuaw quyen hay khong
                var permission = _roleService.CheckPermission(listRole, _claim.Value);
                if (permission || listRole.Contains("admin"))
                {
                    context.Result = new AcceptedResult();
                }
                else
                {
                    context.Result = new ForbidResult();
                }
            }
            else context.Result = new ForbidResult();
        }
    }
}