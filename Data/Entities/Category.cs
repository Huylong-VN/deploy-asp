﻿using System.Collections.Generic;

namespace Solution.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}