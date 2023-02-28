using Framework.Lib.Pages;

namespace Tests.UiTests;

[TestFixture]
public class ProductTest : BaseTest
{
    [SetUp]
    public void SetUp()
    {
        LoginService.Login(ManagerUser);
        LoginPage.ProductLink.Click();
    }
    
    [Test]
    public void VerifyManagerCanCreateProduct()
    {
        ProductService.CreateProduct(Product);

        var nameOfCreatedProduct = ProductPage.ProductName(ProductPage.AmountOfRows);

        var amount = ProductPage.AmountOfRows;
        
        Assert.That(nameOfCreatedProduct.Text, Is.EqualTo(Product.Name));
    }
    
    [TearDown]
    public void LogOut()
    {
        LoginService.LogOut();
    }
}