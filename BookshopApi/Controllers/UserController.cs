using System.Security.Claims;
using BookshopApi.Entities;
using BookshopApi.Models;
using BookshopApi.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookshopApi.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;


    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("api/users/")]
    public IEnumerable<User> GetAllUser()
    {
        return _userService.GetAllUsers();
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    [Route("api/users/{id:int}")]
    public User GetUser(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        var user = _userService.GetUser(id, role, loggedId);

        return user ?? throw new BadHttpRequestException("Role is not allowed to perform request", 400);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Customer")]
    [Route("api/users")]
    public void UpdateUser([FromBody]User user)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        var isUpdated = _userService.UpdateUser(user, role, loggedId);
        
        if(!isUpdated) throw new BadHttpRequestException("Role is not allowed to perform request", 400);
    }

    [HttpPost]
    [Route("api/users")]
    public User CreateUser([FromBody]User user)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin"))
            {
                _userService.CreateUser(user);
                return user;
            }
            else
            {
                throw new BadHttpRequestException("Role is not allowed to perform request", 400);
            }
        }
        user.Auth.Role = Role.Customer;
        _userService.CreateUser(user);

        return user;
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Customer")]
    [Route("api/users/{id:int}")]
    public void DeleteUser(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);
        
        var isDeleted = _userService.DeleteUser(id, role, loggedId);
        
        if(!isDeleted) throw new BadHttpRequestException("Role is not allowed to perform request", 400);
    }
}