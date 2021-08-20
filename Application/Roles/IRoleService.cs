using Solution.ViewModels.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Application.Roles
{
    public interface IRoleService
    {
        Task<List<RoleVM>> getAll();

        bool CheckPermission(string[] listroles, string action);
    }
}