using System.Net;

namespace Tests.ApiTests;

public class Products : BaseTest
{
    [Test, Order(1)]
    public void VerifyCreatingNewProduct()
    {
        Assert.That(ProductApiService.PostProduct(Product).StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test, Order(2)]
    public void VerifyGettingCreatedProduct()
    {
        Assert.That(ProductApiService.GetProduct(Product.Id), Is.EqualTo(Product));
    }
    
    [Test, Order(3)]
    public void VerifyUpdatingExistedProduct()
    {
        Product.Name = "Updated";
        Assert.That(ProductApiService.UpdateProduct(Product).StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(ProductApiService.GetProduct(Product.Id), Is.EqualTo(Product));
    }
    
    [Test, Order(4)]
    public void VerifyDeletingExistedProduct()
    {
        Assert.That(ProductApiService.DeleteProduct(Product.Id).StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}