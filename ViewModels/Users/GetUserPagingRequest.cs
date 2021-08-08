using Solution.ViewModels.Common;

namespace Solution.ViewModels.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}