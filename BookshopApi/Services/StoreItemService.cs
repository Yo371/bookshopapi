using BookshopApi.DataAccess;
using BookshopApi.Entities;
using Commons.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IStoreItemService
{
    Task<IEnumerable<Store>> GetAllStoreItemsAsync();

    Task<Store> GetStoreItemAsync(int id);

    Task UpdateStoreItemAsync(Store storeItemsEntity);

    Task CreateStoreItemAsync(Store storeItemsEntity);

    Task DeleteStoreItemAsync(int id);
}

public class StoreItemService : IStoreItemService
{
    private readonly BookShopContext _context;

    public StoreItemService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Store>> GetAllStoreItemsAsync()
    {
        try
        {
            return await _context.StoreItems.Include(p => p.Product).Select(p => p.Adapt<Store>()).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all store items.", ex);
        }
    }
    
    public async Task<Store> GetStoreItemAsync(int id)
    {
        try
        {
            return (await _context.StoreItems.Include(p => p.Product).FirstOrDefaultAsync(e => e.Id == id))
                .Adapt<Store>();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the store item with ID {id}.", ex);
        }
    }
    
    public async Task UpdateStoreItemAsync(Store storeItem)
    {
        try
        {
            var storeItemsEntity = storeItem.Adapt<StoreItemEntity>();
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == storeItemsEntity.Product.Id);
            storeItemsEntity.Product = product;
            _context.StoreItems.Update(storeItemsEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updatinh store item.", ex);
        }
    }
    
    public async Task CreateStoreItemAsync(Store storeItem)
    {
        try
        {
            var storeItemsEntity = storeItem.Adapt<StoreItemEntity>();
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == storeItemsEntity.Product.Id);
            storeItemsEntity.Product = product;
            _context.StoreItems.Add(storeItemsEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating store item.", ex);
        }
    }
    
    public async Task DeleteStoreItemAsync(int id)
    {
        try
        {
            _context.StoreItems.Remove(_context.StoreItems.Find(id));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting store item.", ex);
        }
    }
}

