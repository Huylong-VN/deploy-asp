using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solution.Application.Category;
using Solution.Application.Products;
using Solution.Controllers;
using Solution.ViewModels.Categories;
using Solution.ViewModels.Common;
using Solution.ViewModels.ProductImages;
using Solution.ViewModels.Products;
using System;
using System.Threading.Tasks;

namespace SolutionForBusiness.BackEndApi.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        public readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 6)
        {
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                categoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var product = await _productService.GetProductPaging(request);
            return Ok(product);
        }

        [HttpPost("image")]
        public async Task<IActionResult> AddImage([FromForm] ProductImageCreateRequest request)
        {
            var result = await _productService.AddImage(request);
            if (result > 0) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            var result = await _productService.Create(request);
            if (result > 0) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var result = await _productService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPost("categoryassign")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CategoryAssign([FromBody] CategoryAssignRequest request)
        {
            var result = await _productService.CategoryAssign(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet("categoryassign/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetCategoryAssignRequest(int id)
        {
            var productObj = await _productService.GetById(id);
            if (productObj == null) return BadRequest("null");
            var categoryObj = await _categoryService.GetAll();
            var categoriesAssignRequest = new CategoryAssignRequest();

            foreach (var category in categoryObj)
            {
                categoriesAssignRequest.Categories.Add(new SelectedItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productObj.Categories.Contains(category.Name)
                });
            }

            return Ok(categoriesAssignRequest);
        }

        [HttpDelete("ProductImage/{imageId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            var result = await _productService.RemoveImage(imageId);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet("ProductImage/{productId}")]
        public async Task<IActionResult> ListImagesByProductId(int productId)
        {
            var result = await _productService.ListImagesByProductId(productId);
            return Ok(result);
        }

        [HttpGet("Detail/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetai(int Id)
        {
            var result = await _productService.GetById(Id);
            return Ok(result);
        }
    }
}