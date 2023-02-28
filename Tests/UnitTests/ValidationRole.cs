using BookshopApi.Entities;
using BookshopApi.Services;
using Commons.Models;

namespace Tests.UnitTests;

[TestFixture]
public class ValidationRole : BaseTest
{
    private IUserService _userService;
    
    [OneTimeSetUp]
    public void CreateService()
    {
        _userService = new UserService(BookShopContext);
    }

    [Test]
    public void VerifyAdminCanGetAnyUser()
    {
        var user = _userService.GetUserAsync(2, Role.Admin.ToString(), "1").Result;
        
        Assert.That(user, Is.Not.Null);
    }
    
    [Test]
    public void VerifyAdminCanGetOwnProfile()
    {
        var user = _userService.GetUserAsync(1, Role.Admin.ToString(), "1").Result;
        
        Assert.That(user, Is.Not.Null);
    }
    
    [Test]
    public void VerifyCustomerCanGetOwnProfile()
    {
        var user = _userService.GetUserAsync(1, Role.Customer.ToString(), "1").Result;
        
        Assert.That(user, Is.Not.Null);
    }
    
    [Test]
    public void VerifyCustomerCanNotGetAnyOtherProfile()
    {
        var user = _userService.GetUserAsync(2, Role.Customer.ToString(), "1").Result;
        
        Assert.That(user, Is.Null);
    }
}