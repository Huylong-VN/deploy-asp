using Solution.ViewModels.Carts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Application.Carts
{
    public interface ICartService
    {
        Task<List<CartVM>> GetAllCart(Guid Id);
    }
}