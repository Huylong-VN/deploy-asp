using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solution.Data.EF;
using Solution.Data.Entities;
using Solution.ViewModels.Roles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Application.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly SolutionDbContext _context;

        public RoleService(RoleManager<Role> roleManager, SolutionDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<List<RoleVM>> getAll()
        {
            var role = await _roleManager.Roles.Select(x => new RoleVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToListAsync();
            return role;
        }

        public bool CheckPermission(string[] listroles, string action)
        {
            var query = from rp in _context.RolePermissions
                        join r in _context.Roles on rp.RoleId equals r.Id
                        join p in _context.Permissions on rp.PermissionId equals p.Id
                        where p.Name.Contains(action) && listroles.Contains(r.Name)
                        select new { rp, r, p };

            if (query.Count() > 0) return true;
            return false;
        }
    }
}