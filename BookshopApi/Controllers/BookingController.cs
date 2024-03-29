﻿using System.Security.Claims;
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
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the all bookings.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);
        
        try
        {
            if (id < 1)
            {
                return BadRequest("Invalid ID");
            }

            var booking = await _bookingService.GetBookingAsync(id, role, loggedId);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the booking with id = {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"An error occurred while getting the booking with id = {id}: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the booking with id = {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> UpdateBooking([FromBody] Booking booking)
    {
        if (booking == null)
        {
            return BadRequest("Booking entity cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid booking entity.");
        }

        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);

        try
        {
            var isUpdated = await _bookingService.UpdateBookingAsync(booking, role, loggedId);

            return isUpdated
                ? StatusCode(StatusCodes.Status202Accepted, "The booking updated.")
                : StatusCode(StatusCodes.Status403Forbidden, "The user is not allowed to perform this action.");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the booking with id = {booking.Id}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the booking with id = {booking.Id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while creating the booking with id = {booking.Id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Manager,Customer")]
    [Route("api/bookings/{id:int}")]
    public async Task<ActionResult> DeleteBooking(int id)
    {
        var loggedId = User.FindFirstValue(ClaimTypes.SerialNumber);
        var role = User.FindFirstValue(ClaimTypes.Role);
    

        try
        {
            if (id < 1)
            {
                return BadRequest("Invalid ID");
            }

            bool isDeleted = await _bookingService.DeleteBookingAsync(id, role, loggedId);

            return isDeleted 
                    ? NoContent() 
                    : StatusCode(StatusCodes.Status403Forbidden, "The user is not allowed to perform this action.");
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the booking with id = {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
        }
    }
}