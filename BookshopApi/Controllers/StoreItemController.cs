using BookshopApi.DataAccess;
using BookshopApi.Services;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookshopApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoreItemController : Controller
{
    private readonly IStoreItemService _storeItemService;
    private readonly ILogger<StoreItemController> _logger;

    public StoreItemController(BookShopContext context, IStoreItemService storeItemService,
                               ILogger<StoreItemController> logger)
    {
        _storeItemService = storeItemService ?? throw new ArgumentNullException(nameof(storeItemService));
        _logger = logger;
        //Throw exception in case of null
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems/")]
    public async Task<ActionResult<IEnumerable<Store>>> GetAllStoreItems()
    {
        try
        {
            var storeItems = await _storeItemService.GetAllStoreItemsAsync();
            if (storeItems == null || !storeItems.Any())
            {
                return NoContent();
            }
            return Ok(storeItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the store items.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems/{id}")]
    public async Task<ActionResult<Store>> GetStoreItem(int id)
    {
        try
        {
            if (id < 1)
            {
                return BadRequest("Invalid ID");
            }
            //Validate the input parameters to ensure they are valid before calling
            var storeItem = await _storeItemService.GetStoreItemAsync(id);
            if (storeItem == null)
            {
                return NotFound();
            }
            return Ok(storeItem);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the store item with id = {id}.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the store item with id = {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems")]
    public async Task<ActionResult> UpdateStoreItem([FromBody]Store storeItem)
    {
        try
        {
            if (storeItem == null || storeItem.Id < 1)
            {
                return BadRequest("Invalid store item data.");
            }
            await _storeItemService.UpdateStoreItemAsync(storeItem);
            return StatusCode(StatusCodes.Status202Accepted, "The store item updated.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the store item.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the store item.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems")]
    public async Task<ActionResult> CreateStoreItem([FromBody] Store storeItemsEntity)
    {
        {
            try
            {
                if (storeItemsEntity == null)
                {
                    return BadRequest("Store item cannot be null");
                }
                await _storeItemService.CreateStoreItemAsync(storeItemsEntity);

                return StatusCode(StatusCodes.Status201Created, storeItemsEntity);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the store item.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the store item.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/storeitems/{id:int}")]
    public async Task<ActionResult> DeleteStoreItem(int id)
    {
        try
        {
            if (id < 1)
            {
                return BadRequest("Invalid ID");
            }
            await _storeItemService.DeleteStoreItemAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the store item with id = {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the store item with id = {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}