using Framework.Lib.Configuration;
using OpenQA.Selenium;
using RPFramework.Core.Utils;
using SeleniumExtras.WaitHelpers;

namespace Framework.Lib.Pages;

public class PageBase
{
    protected IWebDriver browser;
    public static int LoadTimeout = 3000;

    public Element ProductLink => new Element(By.XPath("//a[@id='product_link']"));

    public Element AlertMessage => new Element(By.XPath("//*[@class = 'toast-message']"));

    public PageBase()
    {
        browser = Browser.Instance;
    }


    public void Open()
    {
        var url = ConfigManager.BrowserOptions.BookshopUrl;
        browser.Navigate().GoToUrl(url);
    }

}