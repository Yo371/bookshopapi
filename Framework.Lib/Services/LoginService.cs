using Commons.Models;
using Framework.Lib.Pages;
using Framework.Lib.Utils;

namespace Framework.Lib.Services;

public class LoginService
{
    private LoginPage _loginPage;

    public LoginService()
    {
        _loginPage = new LoginPage();
    }

    public void Login(UserLogin userLogin)
    {
        _loginPage.LoginInput.SendKeys(userLogin.Username);
        _loginPage.PasswordInput.SendKeys(userLogin.Password);
        _loginPage.LoginButton.Click();
    }
    
    public void LogOut()
    {
        _loginPage.Logout.Click();
        Wait.For(() => _loginPage.LoginInput.Displayed);
    }
}