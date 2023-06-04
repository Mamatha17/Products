using Homework.Web.Models;
using Homework.Web.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace Homework.Web.Tests;

[TestFixture]
class Tests
{
    IProductService service;
    Mock<HttpMessageHandler> httpMessageHandlerMock;
    HttpResponseMessage httpResponseMessage;

    [SetUp]
    public void setUp()
    {
        httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://dummyjson.com/")
        };
        service = new ProductsService(httpClient);
    }

    [Test]
    public async Task Test_GetProducts_Success()
    {
        //Create Response Wrapper
        List<Products>? productList = new List<Products>() {
            new Products { Title = "iPhone 9", Brand = "Apple",Price = 549, DiscountPercentage = 12.96M, Rating = 4.69M },
            new Products { Title = "Samsung 9", Brand = "Samsung",Price = 400, DiscountPercentage = 11.96M, Rating = 3.1M },
            new Products { Title = "iPhone 5", Brand = "Apple",Price = 549, DiscountPercentage = 9.96M, Rating = 3.69M },
        };

        ProductData pData = new ProductData()
        {
            Products = productList
        };

        //SetUp Mock
        SetMock(pData, HttpStatusCode.OK);

        //Act
        ProductData result = await service.GetProducts();

        //Assert
        Assert.IsInstanceOf<ProductData>(result);

        Assert.That(result.Products.Count, Is.EqualTo(2), "Selected Products are 2");
        Assert.That(result.TrendingProduct, Is.EqualTo("iPhone 9"), "Trending Product is iPhone 9");

        string[] selectedProductTitles = { "iPhone 9", "Samsung 9" };
        Assert.IsTrue(selectedProductTitles.Contains(result.Products[0].Title), "Product 1 is " + result.Products[0].Title);
        Assert.IsTrue(selectedProductTitles.Contains(result.Products[1].Title), "Product 2 is " + result.Products[1].Title);

        Assert.IsFalse(result.Products[0].Title == "iPhone 5", "Not Selected Product 1 is iPhone 5");
        Assert.IsFalse(result.Products[1].Title == "iPhone 5", "Not Selected Product 2 is iPhone 5");
    }

    [Test]
    public async Task Test_GetProducts_Success_EmptyProducts()
    {
        //Create Response Wrapper
        ProductData pData = new ProductData()
        {
            Products = new List<Products>()
        };

        //SetUp Mock
        SetMock(pData, HttpStatusCode.OK);

        //Act
        ProductData result = await service.GetProducts();

        //Assert
        Assert.IsTrue(result == null);
    }

    [Test]
    public async Task Test_GetProducts_Failure()
    {
        //Setup Mock
        SetMock(null, HttpStatusCode.BadRequest);

        //Act
        ProductData result = await service.GetProducts();

        //Assert
        Assert.IsTrue(result == null);
    }

    private void SetMock(ProductData content, HttpStatusCode statusCode)
    {
        //Create mock response
        httpResponseMessage = new()
        {
            StatusCode = statusCode,
            Content = JsonContent.Create(content)
        };

        //Set up the SendAsync method behavior.
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);
    }
}