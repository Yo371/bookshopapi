using Commons.Models;

namespace Tests.Models;

public class ProductFactory
{
    public static Product GetPredefinedProduct()
    {
        return new Product()
        {
            Name = "Name",
            Description = "Description",
            Author = "Author",
            ImagePath = "Path",
            Price = 12,
        };
    } 
}