﻿using System.Net;
using Commons.Models;

namespace Tests.ApiTests;

public class Products : BaseTest
{
    private Product _product;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _product = new Product()
        {
            Name = "Name",
            Description = "Description",
            Author = "Author",
            ImagePath = "Path",
            Price = 12,
        };
        
        ProductApiService.Authenticate(new UserLogin() { Username = "manager", Password = "123" });
    }
    
    [Test, Order(1)]
    public void VerifyCreatingNewProduct()
    {
        var response = ProductApiService.PostProduct(_product);
        Assert.That(response.IsSuccessful, Is.True);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test, Order(2)]
    public void VerifyGettingCreatedProduct()
    {
        Assert.That(ProductApiService.GetProduct(_product.Id), Is.EqualTo(_product));
    }
    
    [Test, Order(3)]
    public void VerifyUpdatingExistedProduct()
    {
        _product.Name = "Updated";
        Assert.That(ProductApiService.UpdateProduct(_product).IsSuccessful, Is.True);
        Assert.That(ProductApiService.GetProduct(_product.Id), Is.EqualTo(_product));
    }
    
    [Test, Order(4)]
    public void VerifyDeletingExistedProduct()
    {
        var response = ProductApiService.DeleteProduct(_product.Id);
        Assert.That(response.IsSuccessful, Is.True);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}