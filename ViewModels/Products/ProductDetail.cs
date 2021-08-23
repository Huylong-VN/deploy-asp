using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.ViewModels.Products
{
    public class ProductDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
        public string CategoryName { set; get; }

        public DateTime DateCreated { get; set; }
    }
}