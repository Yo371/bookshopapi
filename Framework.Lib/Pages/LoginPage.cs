using OpenQA.Selenium;

namespace Framework.Lib.Pages;

public class LoginPage : PageBase
{
    public Element LoginInput => new Element(By.XPath("//input[@id='login']"));
    
    public Element PasswordInput => new Element(By.XPath("//input[@id='password']"));
    
    public Element LoginButton => new Element(By.XPath("//button[@id='btnLogin']"));
    
    public Element Title => new Element(By.XPath("//h1[@id='Title']"));
    
    public Element Logout => new Element(By.XPath("//a[@id='logout']"));
}