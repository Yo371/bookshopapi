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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Store))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Store))]
    [Route("api/storeitems/")]
    public async Task<ActionResult<IEnumerable<Store>>> GetAllStoreItems()
    {
        try
        {
            var storeItems = await _storeItemService.GetAllStoreItems();
            return Ok(storeItems);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting the store items.");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Store))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Store))]
    [Route("api/storeitems/{id}")]
    public async Task<ActionResult<Store>> GetStoreItem(int id)
    {
        try
        {
            var storeItem = await _storeItemService.GetStoreItem(id);
            return Ok(storeItem);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting the store item.");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Store))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Store))]
    [Route("api/storeitems")]
    public async Task<ActionResult> UpdateStoreItem([FromBody]Store storeItemsEntity)
    {
        try
        {
            await _storeItemService.UpdateStoreItem(storeItemsEntity);
            return StatusCode(StatusCodes.Status202Accepted, "The store item updated.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the store item.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Store))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Store))]
    [Route("api/storeitems")]
    public async Task<ActionResult> CreateStoreItem([FromBody]Store storeItemsEntity)
    {
        try
        {
            await _storeItemService.CreateStoreItem(storeItemsEntity);

            return StatusCode(StatusCodes.Status201Created, storeItemsEntity);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the store item.");
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Store))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Store))]
    [Route("api/storeitems/{id:int}")]
    public async Task<ActionResult> DeleteStoreItem(int id)
    {
        try
        {
            await _storeItemService.DeleteStoreItem(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the store item.");
        }
    }
}