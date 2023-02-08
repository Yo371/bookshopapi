using System.Net;

namespace Tests.ApiTests;

[TestFixture]
public class Users : BaseTest
{

    [Test, Order(1)]
    public void VerifyCreatingNewUser()
    {
        Assert.That(UserApiService.PostUser(User).StatusCode, Is.EqualTo(HttpStatusCode.OK));

    }
    
    [Test, Order(2)]
    public void VerifyGettingCreatedUser()
    {
        Assert.That(UserApiService.GetUser(User.Id), Is.EqualTo(User));
    }
    
    [Test, Order(3)]
    public void VerifyUpdatingExistedUser()
    {
        User.Name = "Updated";
        Assert.That(UserApiService.UpdateUser(User).StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(UserApiService.GetUser(User.Id), Is.EqualTo(User));
    }
    
    [Test, Order(4)]
    public void VerifyDeletingExistedUser()
    {
        Assert.That(UserApiService.DeleteUser(User.Id).StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}