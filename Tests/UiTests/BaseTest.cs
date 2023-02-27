using Commons.Models;
using Framework.Lib;
using Framework.Lib.Pages;
using Framework.Lib.Services;

namespace Tests.UiTests;

public class BaseTest
{
    protected LoginPage LoginPage => new LoginPage();
    
    protected ProductPage ProductPage => new ProductPage();
    
    protected LoginService LoginService => new LoginService();

    protected ProductService ProductService => new ProductService();

    protected readonly UserLogin AdminUser = new UserLogin()
    {
        Username = "goga",
        Password = "123"
    };
    
    protected readonly UserLogin ManagerUser = new UserLogin()
    {
        Username = "manager",
        Password = "123"
    };

    protected Product Product = new Product()
    {
        Name = "Treasure island",
        Author = "R. Stevenson",
        Description = "description",
        Price = 12,
        ImagePath = "dsfsdf.jpg"
    };
    
    [OneTimeSetUp]
    public void RunBeforeAnyTest()
    {
        LoginPage.Open();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTest()
    {
        Browser.CloseBrowser();
    }
}