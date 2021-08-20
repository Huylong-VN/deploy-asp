using Solution.ViewModels.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Application.Permission
{
    public interface IPermissionService
    {
        Task<List<PermissionVM>> GetAll();
    }
}