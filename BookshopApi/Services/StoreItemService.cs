using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IStoreItemService
{
    IEnumerable<Store> GetAllStoreItems();

    Store GetStoreItem(int id);

    void UpdateStoreItem(Store storeItemsEntity);

    void CreateStoreItem(Store storeItemsEntity);

    void DeleteStoreItem(int id);
}

public class StoreItemService : IStoreItemService
{
    private readonly BookShopContext _context;

    public StoreItemService(BookShopContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Store> GetAllStoreItems()
    {
        return _context.StoreItems.Include(p => p.Product).Select(p => p.Adapt<Store>()).ToList();
    }
    
    public Store GetStoreItem(int id)
    {
        return _context.StoreItems.Include(p => p.Product).First(e => e.Id == id).Adapt<Store>();
    }
    
    public void UpdateStoreItem(Store storeItem)
    {
        var storeItemsEntity = storeItem.Adapt<StoreItemEntity>();
        var product = _context.Products.First(e => e.Id == storeItemsEntity.Product.Id);
        storeItemsEntity.Product = product;
        _context.StoreItems.Update(storeItemsEntity);
        _context.SaveChanges();
    }
    
    public void CreateStoreItem(Store storeItem)
    {
        var storeItemsEntity = storeItem.Adapt<StoreItemEntity>();
        var product = _context.Products.First(e => e.Id == storeItemsEntity.Product.Id);
        storeItemsEntity.Product = product;
        _context.StoreItems.Add(storeItemsEntity);
        _context.SaveChanges();
    }
    
    public void DeleteStoreItem(int id)
    {
        _context.StoreItems.Remove(_context.StoreItems.Find(id));
        _context.SaveChanges();
    }
}

