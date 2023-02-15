using System.Net;
using BookshopApi.Entities;
using BookshopApi.Models;

namespace Tests.ApiTests;

[TestFixture]
public class Users : BaseTest
{
    private User _user;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _user = new User()
        {
            Name = "TestUser",
            Address = "Street 3",
            Email = "TestUser@mail.com",
            Auth = new Auth()
            {
                Role = Role.Customer
            },
            Login = "TestUser1",
            Password = "123",
            Phone = 43534532
        };
    }

    [Test, Order(1)]
    public void VerifyCreatingNewUser()
    {
        Assert.That(UserApiService.PostUser(_user).IsSuccessful, Is.True);
    }
    
    [Test, Order(2)]
    public void VerifyGettingCreatedUser()
    {
        Assert.That(UserApiService.GetUser(_user.Id), Is.EqualTo(_user));
    }
    
    [Test, Order(3)]
    public void VerifyUpdatingExistedUser()
    {
        _user.Name = "Updated";
        Assert.That(UserApiService.UpdateUser(_user).IsSuccessful, Is.True);
        Assert.That(UserApiService.GetUser(_user.Id), Is.EqualTo(_user));
    }
    
    [Test, Order(4)]
    public void VerifyDeletingExistedUser()
    {
        Assert.That(UserApiService.DeleteUser(_user.Id).IsSuccessful, Is.True);
    }
}