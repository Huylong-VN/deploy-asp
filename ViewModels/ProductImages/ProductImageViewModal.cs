using System;

namespace Solution.ViewModels.ProductImages
{
    public class ProductImageViewModal
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public DateTime DateCreated { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
    }
}