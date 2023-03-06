using BookshopUi.Services;
using Commons.Models;
using Commons.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace BookshopUi.Controllers;

public class BookingController : BookShopController
{
    private readonly IBookingApiService _bookingApiService;

    public BookingController(IBookingApiService bookingApiService, IValidationService validationService) : base(
        validationService)
    {
        _bookingApiService = bookingApiService ?? throw new ArgumentNullException(nameof(bookingApiService)); ;
        _bookingApiService.SetClient(new RestClient(Constants.ApiUrl));
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Booking> allBookings;

        if (IsManager)
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            allBookings = await _bookingApiService.GetAllBookings();
            return View(allBookings);
        }

        if (IsCustomer)
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            allBookings = await _bookingApiService.GetAllBookings(ValidationModel.Id);
            return View(allBookings);
        }

        return RedirectToAction("Forbidden", "User");
    }

    public IActionResult Create()
    {
        if (IsManager)
        {
            return View();
        }

        return RedirectToAction("Forbidden", "User");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (IsManager)
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            await _bookingApiService.PostBooking(booking);
            SuccessNotification("Booking created successfully");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("Forbidden", "User");
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (IsManager)
        {
            if (id == 0)
            {
                return NotFound();
            }

            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            var bookingFromDb = await _bookingApiService.GetBooking(id);
            if (bookingFromDb == null)
            {
                return NotFound();
            }

            return View(bookingFromDb);
        }

        return RedirectToAction("Forbidden", "User");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Booking booking)
    {
        if (IsManager)
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            await _bookingApiService.UpdateBooking(booking);
            SuccessNotification("Booking updated successfully");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("Forbidden", "User");
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (IsManager)
        {
            if (id == 0)
            {
                return NotFound();
            }

            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            var bookingFromDb = await _bookingApiService.GetBooking(id);
            if (bookingFromDb == null)
            {
                return NotFound();
            }

            return View(bookingFromDb);
        }

        return RedirectToAction("Forbidden", "User");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        if (IsManager)
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            var productFromDb = await _bookingApiService.GetBooking(id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            await _bookingApiService.DeleteBooking(id);
            SuccessNotification("Booking deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("Forbidden", "User");
    }
}