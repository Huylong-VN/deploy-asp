using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Solution.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public List<UserRole> UserRoles { set; get; }
        public List<RolePermission> RolePermissions { set; get; }
    }
}