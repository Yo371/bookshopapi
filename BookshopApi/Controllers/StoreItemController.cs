using BookshopApi.DataAccess;
using BookshopApi.Services;
using Commons.Models;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems/")]
    public async Task<ActionResult<IEnumerable<Store>>> GetAllStoreItems()
    {
        try
        {
            var storeItems = await _storeItemService.GetAllStoreItemsAsync();
            return Ok(storeItems);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting the store items.");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems/{id}")]
    public async Task<ActionResult<Store>> GetStoreItem(int id)
    {
        try
        {
            var storeItem = await _storeItemService.GetStoreItemAsync(id);
            return Ok(storeItem);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting the store item.");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems")]
    public async Task<ActionResult> UpdateStoreItem([FromBody]Store storeItemsEntity)
    {
        try
        {
            await _storeItemService.UpdateStoreItemAsync(storeItemsEntity);
            return StatusCode(StatusCodes.Status202Accepted, "The store item updated.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the store item.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems")]
    public async Task<ActionResult> CreateStoreItem([FromBody]Store storeItemsEntity)
    {
        try
        {
            await _storeItemService.CreateStoreItemAsync(storeItemsEntity);

            return StatusCode(StatusCodes.Status201Created, storeItemsEntity);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the store item.");
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems/{id:int}")]
    public async Task<ActionResult> DeleteStoreItem(int id)
    {
        try
        {
            await _storeItemService.DeleteStoreItemAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the store item.");
        }
    }
}