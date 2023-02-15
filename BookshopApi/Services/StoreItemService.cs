using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IStoreItemService
{
    Task<IEnumerable<Store>> GetAllStoreItems();

    Task<Store> GetStoreItem(int id);

    Task UpdateStoreItem(Store storeItemsEntity);

    Task CreateStoreItem(Store storeItemsEntity);

    Task DeleteStoreItem(int id);
}

public class StoreItemService : IStoreItemService
{
    private readonly BookShopContext _context;

    public StoreItemService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Store>> GetAllStoreItems()
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
    
    public async Task<Store> GetStoreItem(int id)
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
    
    public async Task UpdateStoreItem(Store storeItem)
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
    
    public async Task CreateStoreItem(Store storeItem)
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
    
    public async Task DeleteStoreItem(int id)
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

