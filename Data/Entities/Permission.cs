using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Data.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<RolePermission> RolePermissions { set; get; }
    }
}