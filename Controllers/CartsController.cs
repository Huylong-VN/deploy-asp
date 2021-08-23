using Microsoft.AspNetCore.Mvc;
using Solution.Application.Carts;
using System;
using System.Threading.Tasks;

namespace Solution.Controllers
{
    public class CartsController : BaseController
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCart(Guid Id)
        {
            var result = await _cartService.GetAllCart(Id);
            return Ok(result);
        }
    }
}