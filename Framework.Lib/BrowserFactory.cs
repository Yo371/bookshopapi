using Framework.Lib.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Framework.Lib
{
    internal class BrowserFactory
    {
        public static IWebDriver GetChromeDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(),
                VersionResolveStrategy.MatchingBrowser);
            return new ChromeDriver();
        }

        public static IWebDriver GetFirefoxDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
            return new FirefoxDriver();
        }

        public static IWebDriver GetEdgeDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
            return new EdgeDriver();
        }
    }
}