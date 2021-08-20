using System;
using System.Collections.Generic;

namespace Solution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        public List<Cart> Carts { set; get; }
        public List<ProductCategory> ProductCategories { set; get; }
        public List<OrderProduct> OrderProducts { set; get; }
        public List<ProductImage> ProductImages { set; get; }
    }
}