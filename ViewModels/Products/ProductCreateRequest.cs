using Microsoft.AspNetCore.Http;
using System;

namespace Solution.ViewModels.Products
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }
    }
}