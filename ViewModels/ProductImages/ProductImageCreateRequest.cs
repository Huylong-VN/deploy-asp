using Microsoft.AspNetCore.Http;

namespace Solution.ViewModels.ProductImages
{
    public class ProductImageCreateRequest
    {
        public int productId { set; get; }
        public string Caption { get; set; }
        public bool isDefault { get; set; }
        public int SortOrder { get; set; }
        public IFormFile Image { get; set; }
    }
}