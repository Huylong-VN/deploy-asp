using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Data.Entities
{
    public class RolePermission
    {
        public Role Role { get; set; }
        public Guid RoleId { set; get; }
        public Permission Permission { get; set; }
        public int PermissionId { set; get; }
    }
}