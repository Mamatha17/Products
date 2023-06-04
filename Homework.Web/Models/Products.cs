namespace Homework.Web.Models
{
    public class ProductData
    {
        public List<Products> Products { get; set; }
        public string TrendingProduct { get; set; }
    }
    public class Products
    {
        public string Title { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Rating { get; set; }
    }
}
