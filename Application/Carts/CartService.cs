using Microsoft.EntityFrameworkCore;
using Solution.Data.EF;
using Solution.ViewModels.Carts;
using Solution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Application.Carts
{
    public class CartService : ICartService
    {
        private readonly SolutionDbContext _context;

        public CartService(SolutionDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartVM>> GetAllCart(Guid Id)
        {
            return await _context.Carts.Where(x => x.UserId == Id).Select(x => new CartVM()
            {
                Id = x.Id,
                DateCreated = x.DateCreated,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToListAsync();
        }
    }
}