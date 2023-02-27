using Framework.Lib.Pages;
using NUnit.Framework;

namespace Tests.UiTests;

[TestFixture]
public class LoginTest : BaseTest
{
    [Test]
    public void VerifyUserCanLogIn()
    {
        LoginService.Login(AdminUser);
        
        Assert.That(LoginPage.Title.Text, Is.EqualTo("Welcome goga"));
    }

    [TearDown]
    public void LogOut()
    {
        LoginService.LogOut();
    }
}