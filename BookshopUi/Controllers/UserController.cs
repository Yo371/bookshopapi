using BookshopUi.Services;
using Commons.Models;
using Commons.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace BookshopUi.Controllers;

public class UserController : BookShopController
{
    private readonly IUserApiService _userApiService;

    public UserController(IUserApiService userApiService, IValidationService validationService) : base(
        validationService)
    {
        _userApiService = userApiService;
        _userApiService.SetClient(new RestClient(Constants.ApiUrl));
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<User> userList;
        if (IsAdmin)
        {
            _userApiService.AddTokenToHeader(ValidationModel.Token);
            userList = await _userApiService.GetAllUsers();
            return View(userList);
        }

        if (IsCustomer)
        {
            _userApiService.AddTokenToHeader(ValidationModel.Token);
            var user = await _userApiService.GetUser(ValidationModel.Id);
            userList = new List<User>() { user };
            return View(userList);
        }

        return RedirectToAction(nameof(Forbidden));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            await _userApiService.PostUser(user);
            SuccessNotification("User created successfully");

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        if (IsAdmin || IsCustomer)
        {
            if (IsCustomer && ValidationModel.Id != id)
                return RedirectToAction(nameof(Forbidden));

            _userApiService.AddTokenToHeader(ValidationModel.Token);
            var userFromDb = await _userApiService.GetUser(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        return Unauthorized();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User user)
    {
        if ((IsAdmin || IsCustomer) && ModelState.IsValid)
        {
            _userApiService.AddTokenToHeader(ValidationModel.Token);
            await _userApiService.UpdateUser(user);
            SuccessNotification("User updated successfully");
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        if (IsAdmin || IsCustomer)
        {
            if (IsCustomer && ValidationModel.Id != id)
                return RedirectToAction("Forbidden");

            _userApiService.AddTokenToHeader(ValidationModel.Token);
            var userFromDb = await _userApiService.GetUser(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        return Unauthorized();
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        if (IsAdmin || IsCustomer)
        {
            if (IsCustomer && ValidationModel.Id != id)
                return RedirectToAction("Forbidden");
            _userApiService.AddTokenToHeader(ValidationModel.Token);
            var userFromDb = await _userApiService.GetUser(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            await _userApiService.DeleteUser(id);
            SuccessNotification("User deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        return Unauthorized();
    }

    public IActionResult Forbidden()
    {
        return View();
    }
}