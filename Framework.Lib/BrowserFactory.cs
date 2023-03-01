using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
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
            
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("window-size=1900,1200");
            return new ChromeDriver(chromeOptions);
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