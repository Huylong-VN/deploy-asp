using System;

namespace Solution.ViewModels.Users
{
    public class GetRefreshTokenRequest
    {
        public Guid Id { get; set; }
        public string refreshToken { get; set; }
    }
}