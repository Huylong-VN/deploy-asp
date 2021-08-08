using System;

namespace Solution.ViewModels.Users
{
    public class UpdateRequest
    {
        public Guid Id { get; set; }
        public Guid userId { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Dob { get; set; }
    }
}