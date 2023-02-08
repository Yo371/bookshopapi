using System.Security.Claims;
using BookshopApi.Models;
using BookshopApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookshopApi.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    [Route("api/bookings/")]
    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookingService.GetAllBookings();
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/{id}")]
    public Booking GetBooking(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);
        
        var booking = _bookingService.GetBooking(id, role, loggedId);

        return booking ?? throw new BadHttpRequestException("Role is not allowed to perform request", 400);
    }

    [HttpPut]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings")]
    public void UpdateBooking([FromBody]Booking bookingEntity)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        var isUpdated = _bookingService.UpdateBooking(bookingEntity, role, loggedId);
        
        if(!isUpdated) throw new BadHttpRequestException("Role is not allowed to perform request", 400);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings")]
    public Booking CreateBooking([FromBody]Booking booking)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);
        
        var isCreated = _bookingService.CreateBooking(booking, role, loggedId);
        
        if(!isCreated) throw new BadHttpRequestException("Role is not allowed to perform request", 400);

        return booking;
    }

    [HttpDelete]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/{id:int}")]
    public void DeleteBooking(int idOfBooking)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);
        
        var isCreated = _bookingService.DeleteBooking(idOfBooking, role, loggedId);
        
        if(!isCreated) throw new BadHttpRequestException("Role is not allowed to perform request", 400);
    }
}