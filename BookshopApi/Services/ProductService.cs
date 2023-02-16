using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<Product> GetProductAsync(int id);

    Task UpdateProductAsync(Product productEntity);

    Task<bool> CreateProductAsync(Product productEntity);

    Task<bool> DeleteProductAsync(int id);
}

public class ProductService : IProductService
{
    private readonly BookShopContext _context;
    
    public ProductService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
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
    
    public async Task<Product> GetProductAsync(int id)
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


    public async Task UpdateProductAsync(Product productEntity)
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
    
    public async Task<bool> CreateProductAsync(Product product)
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
    
    public async Task<bool> DeleteProductAsync(int id)
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

