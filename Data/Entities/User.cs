﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Solution.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { set; get; }
        public string RefreshToken { set; get; }
        public DateTime InValidRefreshToken { set; get; }
        public List<Cart> Carts { set; get; }
        public List<Order> Orders { set; get; }
        public List<UserRole> UserRoles { set; get; }
    }
}