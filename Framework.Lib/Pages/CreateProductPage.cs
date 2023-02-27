using OpenQA.Selenium;

namespace Framework.Lib.Pages;

public class CreateProductPage
{
    public Element NameInput => new Element(By.XPath("//*[@id='product_name']"));
    
    public Element DescriptionInput => new Element(By.XPath("//*[@id='product_description']"));
    
    public Element ImageInput => new Element(By.XPath("//*[@id='product_img']"));
    
    public Element PriceInput => new Element(By.XPath("//*[@id='product_price']"));
    
    public Element AuthorInput => new Element(By.XPath("//*[@id='product_author']"));
    
    public Element CreateButton => new Element(By.XPath("//*[@id='create_product_btn']"));
}