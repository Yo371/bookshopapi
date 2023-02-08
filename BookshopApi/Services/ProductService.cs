using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;

namespace BookshopApi.Services;

public interface IProductService
{
    IEnumerable<Product> GetAllProducts();

    Product GetProduct(int id);

    void UpdateProduct(Product productEntity);

    void CreateProduct(Product productEntity);

    void DeleteProduct(int id);
}

public class ProductService : IProductService
{
    private readonly BookShopContext _context;
    
    public ProductService(BookShopContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products.Select(e => e.Adapt<Product>()).ToList();
    }
    
    public Product GetProduct(int id)
    {
        return _context.Products.Find(id).Adapt<Product>();
    }


    public void UpdateProduct(Product productEntity)
    {
        var product = productEntity.Adapt<ProductEntity>();
        _context.Products.Update(product);
        _context.SaveChanges();
    }
    
    public void CreateProduct(Product product)
    {
        var productEntity = product.Adapt<ProductEntity>();
        _context.Products.Add(productEntity);
        _context.SaveChanges();
        product.Id = productEntity.Id;
    }
    
    public void DeleteProduct(int id)
    {
        _context.Products.Remove(_context.Products.Find(id));
        _context.SaveChanges();
    }
}

