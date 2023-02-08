using System.Net;

namespace Tests.ApiTests;

public class Products : BaseTest
{
    [Test, Order(1)]
    public void CreateProductTest()
    {
        Assert.That(ProductApiService.PostProduct(Product).StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(2)]
    public void GetProductTest()
    {
        Assert.That(ProductApiService.GetProduct(Product.Id), Is.EqualTo(Product));
    }
    
    [Test, Order(3)]
    public void UpdateProductTest()
    {
        Product.Name = "Updated";
        Assert.That(ProductApiService.UpdateProduct(Product).StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(ProductApiService.GetProduct(Product.Id), Is.EqualTo(Product));
    }
    
    [Test, Order(4)]
    public void DeleteProductTest()
    {
        Assert.That(ProductApiService.DeleteProduct(Product.Id).StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}