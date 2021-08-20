using Solution.ViewModels.Common;
using Solution.ViewModels.RefreshTokens;
using Solution.ViewModels.Roles;
using Solution.ViewModels.Users;
using System;
using System.Threading.Tasks;

namespace Solution.Application.Users
{
    public interface IUserService
    {
        Task<ApiResult<UserVM>> Authenticate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<Pagination<UserVM>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<Guid>> Update(UpdateRequest request);

        Task<ApiResult<bool>> UpdatePassword(UpdatePasswordRequest request);

        Task<ApiResult<bool>> Delete(Guid Id);

        Task<ApiResult<bool>> RoleAssign(RoleAssignRequest request);

        Task<ApiResult<UserVM>> getById(Guid Id);

        Task<ApiResult<string>> ResetPassword(Guid Id, string newPassword);

        Task<RefreshToken> GetRefreshToken(GetRefreshTokenRequest refreshToken);
    }
}