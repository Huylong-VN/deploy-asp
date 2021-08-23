using Solution.ViewModels.Categories;
using Solution.ViewModels.Common;
using Solution.ViewModels.ProductImages;
using Solution.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Application.Products
{
    public interface IProductService
    {
        Task<Pagination<ProductVM>> GetProductPaging(GetProductPagingRequest request);

        Task<int> AddImage(ProductImageCreateRequest request);

        Task<ProductImageViewModal> GetImageById(int productImageId);

        Task<int> Create(ProductCreateRequest request);

        Task<ApiResult<int>> Update(ProductUpdateRequest request);

        Task<ApiResult<int>> Delete(int Id);

        Task<ProductVM> GetById(int Id);

        Task<ApiResult<bool>> RemoveImage(int imageId);

        Task<List<ProductImageViewModal>> ListImagesByProductId(int productId);

        Task<ApiResult<bool>> CategoryAssign(CategoryAssignRequest request);

        Task<List<ProductDetail>> GetDetail(int Id);
    }
}