using Microsoft.AspNetCore.Http;
using System;

namespace Solution.ViewModels.Products
{
    public class ProductUpdateRequest
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public IFormFile image { set; get; }
    }
}