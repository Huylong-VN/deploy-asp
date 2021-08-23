namespace Solution.Data.Entities
{
    public class ProductCategory
    {
        public Product Product { set; get; }
        public int productId { set; get; }
        public Category Category { set; get; }
        public int categoryId { set; get; }
    }
}