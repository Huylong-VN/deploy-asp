﻿using System;
using System.Collections.Generic;

namespace Solution.ViewModels.Products
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
        public List<string> Categories { set; get; } = new List<string>();

        public DateTime DateCreated { get; set; }
    }
}