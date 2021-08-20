using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.ViewModels.RefreshTokens
{
    public class RefreshToken
    {
        public string accessToken { set; get; }
        public string refreshToken { set; get; }
    }
}