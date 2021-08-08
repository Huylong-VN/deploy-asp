using Solution.ViewModels.Common;
using Solution.ViewModels.Products;
using System.Threading.Tasks;

namespace Solution.Application.Products
{
    public interface IProductService
    {
        Task<PagedResult<ProductVM>> GetProductPaging(GetProductPagingRequest request);

        Task<ApiResult<bool>> Create(ProductCreateRequest request);

        Task<ApiResult<bool>> Update(ProductUpdateRequest request);

        Task<ApiResult<bool>> Delete(int Id);

        Task<ProductVM> GetById(int Id);
    }
}