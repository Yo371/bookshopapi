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
        _bookingApiService = bookingApiService;
        _bookingApiService.SetClient(new RestClient(Constants.ApiUrl));
    }

    public IActionResult Index()
    {
        if (ValidationModel.IsCredentialsMatched)
        {
            IEnumerable<Booking> allBookings;
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            if (ValidationModel.Role.Equals(Role.Manager))
            {
                allBookings = _bookingApiService.GetAllBookings();
                return View(allBookings);
            }

            if (ValidationModel.Role.Equals(Role.Customer))
            {
                allBookings = _bookingApiService.GetAllBookings(ValidationModel.Id);
                return View(allBookings);
            }
        }

        return RedirectToAction("Forbidden", "User");
    }

    public IActionResult Create()
    {
        if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
        {
            return View();
        }

        return RedirectToAction("Forbidden", "User");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Booking booking)
    {
        if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            _bookingApiService.PostBooking(booking);
            TempData["success"] = "Booking created successfully";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Forbidden", "User");
    }

    public IActionResult Edit(int id)
    {
        if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
        {
            if (id == 0)
            {
                return NotFound();
            }

            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            var bookingFromDb = _bookingApiService.GetBooking(id);
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
    public IActionResult Edit(Booking booking)
    {
        if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            _bookingApiService.UpdateBooking(booking);
            TempData["success"] = "Booking updated successfully";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Forbidden", "User");
    }

    public IActionResult Delete(int id)
    {
        if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
        {
            if (id == 0)
            {
                return NotFound();
            }

            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            var bookingFromDb = _bookingApiService.GetBooking(id);
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
    public IActionResult DeletePost(int id)
    {
        if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
        {
            _bookingApiService.AddTokenToHeader(ValidationModel.Token);
            var productFromDb = _bookingApiService.GetBooking(id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            _bookingApiService.DeleteBooking(id);
            TempData["success"] = "Booking delete successfully";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Forbidden", "User");
    }
}