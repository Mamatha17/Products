using Homework.Web.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection.Metadata;

namespace Homework.Web.Services
{
    public class ProductsService : IProductService
    {
        private readonly HttpClient productHttpClient;
        private const decimal percentage = 10;
        public ProductsService(HttpClient httpClient)
        {
            productHttpClient = httpClient;
        }

        /// <summary>
        /// Get the Products from the API whose discount percentage is greater than and equal to 10
        /// </summary>
        /// <returns></returns>
        public async Task<ProductData?> GetProducts()
        {
            var responseMessage = await productHttpClient.GetAsync("products");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseStr = await responseMessage.Content.ReadAsStringAsync();

                ProductData totalProducts = JsonConvert.DeserializeObject<ProductData>(responseStr);
                ProductData selectedProducts = new ProductData();
                if (totalProducts != null && totalProducts.Products.Count > 0)
                {
                    List<Products> selectedlist = totalProducts.Products.Where(item => item.DiscountPercentage >= percentage).ToList();
                    selectedProducts.Products = selectedlist;
                    selectedProducts.TrendingProduct = GetTrendingProduct(selectedlist);
                    return selectedProducts;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the trending product based on the highest rating
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private static string GetTrendingProduct(List<Products> products)
        {
            var maxRating = (products.Max(e => e.Rating));
            var trendingItem = products.First(m => m.Rating == maxRating).Title;
            return trendingItem;
        }
    }
}
