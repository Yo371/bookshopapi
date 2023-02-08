using BookshopApi.Entities;
using BookshopApi.Services;

namespace Tests.UnitTests;

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
        var user = _userService.GetUser(2, Role.Admin.ToString(), "1");
        
        Assert.That(user, Is.Not.Null);
    }
    
    [Test]
    public void VerifyAdminCanGetOwnProfile()
    {
        var user = _userService.GetUser(1, Role.Admin.ToString(), "1");
        
        Assert.That(user, Is.Not.Null);
    }
    
    [Test]
    public void VerifyCustomerCanGetOwnProfile()
    {
        var user = _userService.GetUser(1, Role.Customer.ToString(), "1");
        
        Assert.That(user, Is.Not.Null);
    }
    
    [Test]
    public void VerifyCustomerCanNotGetAnyProfile()
    {
        var user = _userService.GetUser(2, Role.Customer.ToString(), "1");
        
        Assert.That(user, Is.Null);
    }
}