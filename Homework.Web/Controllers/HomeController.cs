using Microsoft.AspNetCore.Mvc;
using Homework.Web.Services;

namespace Homework.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<ActionResult> Index()
    {
        try
        {
            var response = await _productService.GetProducts();

            if (response == null)
            {
                return View("There are no products");
            }
            return View(response);
        }
        catch (Exception ex)
        {
            return View("Error " + ex.Message);
        }
    }
}