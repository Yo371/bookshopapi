using System.Collections.ObjectModel;
using System.Drawing;
using Framework.Lib.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace Framework.Lib
{
    public class Element : IWebElement
    {
        protected const double DefaultTimeOutMs = 20000;

        private readonly Lazy<IWebElement> _lazy;

        protected IWebElement WrappedElement => _lazy.Value;

        private readonly By _by;

        public Element(By by)
        {
            _lazy = new Lazy<IWebElement>(() =>  Browser.Instance.FindElement(by));
            _by = by;
        }
        

        public IWebElement FindElement(By by)
        {
            return new Element(by);
        }

       

        public ReadOnlyCollection<IWebElement> FindElements(By by) => WrappedElement.FindElements(by);

        private void WaitForPresence(double timeOut = DefaultTimeOutMs)
        {
            Wait.Explicit().Until(ExpectedConditions.ElementExists(_by));
            Wait.Explicit().Until(ExpectedConditions.ElementIsVisible(_by));
        }

        public void Clear()
        {
            WaitForPresence();
            WrappedElement.Clear();
        }

        public void SendKeys(string text)
        {
            WaitForPresence();
            WrappedElement.SendKeys(text);
        }

        public void Submit() => WrappedElement.Submit();

        public void Click()
        {
            WaitForPresence();
            Wait.Explicit().Until(ExpectedConditions.ElementToBeClickable(_by));
            WrappedElement.Click();
        }

        public void ScrollToElement() =>
            ((IJavaScriptExecutor)Browser.Instance).ExecuteScript("arguments[0].scrollIntoView(true);",
               WrappedElement );

        public void MoveToElement() => new Actions(Browser.Instance).MoveToElement(WrappedElement).Build().Perform();

        public string GetAttribute(string attributeName) => WrappedElement.GetAttribute(attributeName);

        public string GetDomAttribute(string attributeName) => WrappedElement.GetDomAttribute(attributeName);

        public string GetDomProperty(string propertyName) => WrappedElement.GetDomProperty(propertyName);

        public string GetCssValue(string propertyName) => WrappedElement.GetCssValue(propertyName);
        public ISearchContext GetShadowRoot() => WrappedElement.GetShadowRoot();

        public string TagName => WrappedElement.TagName;
        public string Text => WrappedElement.Text;
        public bool Enabled => WrappedElement.Enabled;
        public bool Selected => WrappedElement.Selected;    
        public Point Location => WrappedElement.Location;
        public Size Size => WrappedElement.Size;
        public bool Displayed => WrappedElement.Displayed;

        public void DragAndDropToOffset(int offsetX, int offsetY) => new Actions(Browser.Instance)
            .DragAndDropToOffset(WrappedElement, offsetX, offsetY).Build().Perform();
    }
}
