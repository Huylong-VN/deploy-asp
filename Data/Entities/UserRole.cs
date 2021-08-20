using Microsoft.AspNetCore.Identity;
using System;

namespace Solution.Data.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { set; get; }
        public Role Role { set; get; }
    }
}