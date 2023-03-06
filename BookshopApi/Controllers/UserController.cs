using System.Security.Claims;
using BookshopApi.Services;
using Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookshopApi.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;


    public UserController(IUserService userService,  ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    [Route("api/users/")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
    {
        try
        {
            return Ok(await _userService.GetAllUsersAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the users.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin,Customer")]
    [Route("api/users/{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            var user = await _userService.GetUserAsync(id, role, loggedId);
            return user != null ? Ok(user) : StatusCode(403, "The user is not allowed to perform this action." +
                                                             $"or user with {id} doesn't exist.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the user with id = {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin,Customer")]
    [Route("api/users")]
    public async Task<ActionResult> UpdateUser([FromBody]User user)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            var isUpdated = await _userService.UpdateUserAsync(user, role, loggedId);
            
            return isUpdated
                ? StatusCode(StatusCodes.Status202Accepted, "The user updated.")
                : StatusCode(403, "The user is not allowed to perform this action.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("api/users")]
    public async Task<ActionResult<User>> CreateUser([FromBody]User user)
    {
        try
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    await _userService.CreateUserAsync(user);
                    return  StatusCode(StatusCodes.Status201Created, user);
                }
            }

            user.Auth.Role = Role.Customer; 
            await _userService.CreateUserAsync(user);

            return  StatusCode(StatusCodes.Status201Created, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the user.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "Admin,Customer")]
    [Route("api/users/{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            bool isDeleted = await _userService.DeleteUserAsync(id, role, loggedId);

            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the user with id = {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}