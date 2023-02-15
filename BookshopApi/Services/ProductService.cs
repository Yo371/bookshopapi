using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProducts();

    Task<Product> GetProduct(int id);

    Task UpdateProduct(Product productEntity);

    Task<bool> CreateProduct(Product productEntity);

    Task<bool> DeleteProduct(int id);
}

public class ProductService : IProductService
{
    private readonly BookShopContext _context;
    
    public ProductService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        try
        {
            return await _context.Products.Select(e => e.Adapt<Product>()).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all products.", ex);
        }
    }
    
    public async Task<Product> GetProduct(int id)
    {
        try
        {
            var productEntity = await _context.Products.FindAsync(id);
            return productEntity.Adapt<Product>();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the product with ID {id}.", ex);
        }
    }


    public async Task UpdateProduct(Product productEntity)
    {
        try
        {
            var product = productEntity.Adapt<ProductEntity>();
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating product.", ex);
        }
    }
    
    public async Task<bool> CreateProduct(Product product)
    {
        try
        {
            var productEntity = product.Adapt<ProductEntity>();
            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();
            product.Id = productEntity.Id;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating product.", ex);
        }
    }
    
    public async Task<bool> DeleteProduct(int id)
    {
        try
        {
            _context.Products.Remove(_context.Products.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting product.", ex);
        }
    }
}

