using System.Security.Claims;
using BookshopApi.Services;
using Commons.Models;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
    {
        try
        {
            var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
            var role = User.FindFirstValue(ClaimTypes.Role);
            
            IEnumerable<Booking> bookings;
            if (role.Equals(Role.Customer.ToString()))
            {
                bookings = await _bookingService.GetAllBookingsAsync(int.Parse(loggedId));
            }
            else
            {
                bookings = await _bookingService.GetAllBookingsAsync();
            }
             
            return Ok(bookings);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the booking.");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            var booking = await _bookingService.GetBookingAsync(id, role, loggedId);

            return booking != null ? Ok(booking) : StatusCode(403, "The user is not allowed to perform this action.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the booking.");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings")]
    public async Task<ActionResult> UpdateBooking([FromBody] Booking bookingEntity)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            var isUpdated = await _bookingService.UpdateBookingAsync(bookingEntity, role, loggedId);

            return isUpdated
                ? StatusCode(StatusCodes.Status202Accepted, "The booking updated.")
                : StatusCode(StatusCodes.Status403Forbidden, "The user is not allowed to perform this action.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the booking.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings")]
    public async Task<ActionResult<Booking>> CreateBooking([FromBody] Booking booking)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            var isCreated = await _bookingService.CreateBookingAsync(booking, role, loggedId);

            return isCreated
                ? StatusCode(StatusCodes.Status201Created, booking)
                : StatusCode(StatusCodes.Status403Forbidden, "The user is not allowed to perform this action.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the booking.");
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/{id:int}")]
    public async Task<ActionResult> DeleteBooking(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            bool isDeleted = await _bookingService.DeleteBookingAsync(id, role, loggedId);

            return isDeleted ? NoContent() : StatusCode(403, "The user is not allowed to perform this action.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the booking.");
        }
    }
}