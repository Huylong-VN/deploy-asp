using System;

namespace Solution.ViewModels.Carts
{
    public class CartVM
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
    }
}