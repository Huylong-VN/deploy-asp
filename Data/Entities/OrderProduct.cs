﻿namespace Solution.Data.Entities
{
    public class OrderProduct
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }
}