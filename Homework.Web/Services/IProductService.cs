using Homework.Web.Models;

namespace Homework.Web.Services
{
    public interface IProductService
    {
        public Task<ProductData> GetProducts();
    }
}
