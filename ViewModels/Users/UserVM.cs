using System;
using System.Collections.Generic;

namespace Solution.ViewModels.Users
{
    public class UserVM
    {
        public Guid Id { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> Roles { set; get; }
        public string Token { set; get; }
    }
}