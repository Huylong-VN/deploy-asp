using Solution.ViewModels.Common;

namespace Solution.ViewModels.Products
{
    public class GetProductPagingRequest : PaginationBase
    {
        public string Keyword { get; set; }
        public int? categoryId { set; get; }
    }
}