using System;
using System.Threading;
using Framework.Lib;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RPFramework.Core.Utils
{
    public class Wait
    {
        private const double DefaultTimeOutMs = 15000;

        private const int RetryIntervalMs = 50;

        public static WebDriverWait Explicit(double timeout = DefaultTimeOutMs) => new WebDriverWait(Browser.Instance, TimeSpan.FromMilliseconds(timeout));

        public static void ForPresence(IWebElement webElement, double timeOut = DefaultTimeOutMs) 
            => Wait.For(() => webElement.Displayed, timeOut);

        public static void WhilePresented(IWebElement webElement, double timeOut = DefaultTimeOutMs) 
            => Wait.While(() => webElement.Displayed, timeOut);

        public static void For(Func<bool> condition, double timeOut = 5000)
        {
            var startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < timeOut)
            {
                try
                {
                    if (condition())
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    // skip
                }
                Thread.Sleep(RetryIntervalMs);
            }
        }

        public static void While(Func<bool> condition, double timeOut = DefaultTimeOutMs)
        {
            var startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < timeOut)
            {
                try
                {
                    if (!condition())
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    // skip
                }
                Thread.Sleep(RetryIntervalMs);
            }
        }

        public static void ABit(int timeout = 1500)
        {
            Thread.Sleep(timeout);
        }

        public static void WhileElementMoving(IWebElement element)
        {
            var currentLocation = element.Location;
            while (element.Location != currentLocation)
            {
                currentLocation = element.Location;
                ABit(200);
            }
        }
    }
}
