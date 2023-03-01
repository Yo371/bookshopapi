using System.Globalization;
using Commons.Models;
using Framework.Lib.Pages;
using Framework.Lib.Utils;

namespace Framework.Lib.Services;

public class ProductService
{
    private ProductPage _productPage;

    private CreateProductPage _createProductPage;

    public ProductService()
    {
        _productPage = new ProductPage();
        _createProductPage = new CreateProductPage();
    }

    public void CreateProduct(Product product)
    {
        Wait.For(() => _productPage.CreateProductButton.Displayed);
        _productPage.CreateProductButton.Click();
        _createProductPage.NameInput.SendKeys(product.Name);
        _createProductPage.AuthorInput.SendKeys(product.Author);
        _createProductPage.DescriptionInput.SendKeys(product.Description);
        _createProductPage.PriceInput.SendKeys(product.Price.ToString(CultureInfo.InvariantCulture));
        _createProductPage.ImageInput.SendKeys(product.ImagePath);
        _createProductPage.CreateButton.Click();
        Wait.For(() => _productPage.CreateProductButton.Displayed);
    }
}