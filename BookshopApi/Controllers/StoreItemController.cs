using BookshopApi.DataAccess;
using BookshopApi.Models;
using BookshopApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookshopApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoreItemController : Controller
{
    private readonly IStoreItemService _storeItemService;

    public StoreItemController(BookShopContext context, IStoreItemService storeItemService)
    {
        _storeItemService = storeItemService;
    }

    [HttpGet]
    [Route("api/storeitems/")]
    public IEnumerable<Store> GetAllStoreItems()
    {
        return _storeItemService.GetAllStoreItems();
    }

    [HttpGet]
    [Route("api/storeitems/{id}")]
    public Store GetStoreItem(int id)
    {
        return _storeItemService.GetStoreItem(id);
    }

    [HttpPut]
    [Route("api/storeitems")]
    public void UpdateStoreItem([FromBody]Store storeItemsEntity)
    {
        _storeItemService.UpdateStoreItem(storeItemsEntity);
    }

    [HttpPost]
    [Route("api/storeitems")]
    public void CreateStoreItem([FromBody]Store storeItemsEntity)
    {
        _storeItemService.CreateStoreItem(storeItemsEntity);
    }

    [HttpDelete]
    [Route("api/storeitems/{id:int}")]
    public void DeleteStoreItem(int id)
    {
        _storeItemService.DeleteStoreItem(id);
    }
}