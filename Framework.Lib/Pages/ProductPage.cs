using OpenQA.Selenium;

namespace Framework.Lib.Pages;

public class ProductPage: PageBase
{
    public Element CreateProductButton => new Element(By.XPath("//*[@id='create_product']"));

    public Element ProductName(int row) => new Element(By.XPath($"((//tr)[{row}]/td)[2]"));

    public int AmountOfRows => browser.FindElements(By.XPath($"//tr")).Count;
}