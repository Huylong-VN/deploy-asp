using System;

namespace Solution.ViewModels.Users
{
    public class DeleteRequest
    {
        public Guid userId { get; set; }
        public Guid Id { get; set; }
    }
}