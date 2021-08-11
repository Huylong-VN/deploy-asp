using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solution.Application.Category;
using Solution.Application.Products;
using Solution.Application.Users;
using Solution.ViewModels.Categories;
using Solution.ViewModels.Common;
using Solution.ViewModels.Products;
using System;
using System.Threading.Tasks;

namespace SolutionForBusiness.BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public readonly IUserService _userService;
        public readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, IUserService userService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _userService = userService;
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var usercheck = await _userService.checkRoleUser(request.userId);
            if (usercheck.IsSuccessed)
            {
                var result = await _productService.Create(request);
                if (result.IsSuccessed) return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid userId, int Id)
        {
            var usercheck = await _userService.checkRoleUser(userId);
            if (usercheck.IsSuccessed)
            {
                var result = await _productService.Delete(Id);
                if (result.IsSuccessed) return Ok(result);
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateRequest request)
        {
            var usercheck = await _userService.checkRoleUser(request.userId);
            if (usercheck.IsSuccessed)
            {
                var result = await _productService.Update(request);
                if (result.IsSuccessed) return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("categoryassign/{id}")]
        public async Task<IActionResult> CategoryAssignRequest(int id)
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
    }
}