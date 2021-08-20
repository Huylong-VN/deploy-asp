using Microsoft.EntityFrameworkCore;
using Solution.Data.EF;
using Solution.ViewModels.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Application.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly SolutionDbContext _context;

        public PermissionService(SolutionDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionVM>> GetAll()
        {
            return await _context.Permissions.Select(x => new PermissionVM()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Title = x.Title
            }).ToListAsync();
        }
    }
}