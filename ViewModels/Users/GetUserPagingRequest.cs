using Solution.ViewModels.Common;

namespace Solution.ViewModels.Users
{
    public class GetUserPagingRequest : PaginationBase
    {
        public string Keyword { get; set; }
    }
}