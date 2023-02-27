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

    public IActionResult Index()
    {
        IEnumerable<User> userList;

        if (ValidationModel.IsCredentialsMatched)
        {
            _userApiService.AddTokenToHeader(ValidationModel.Token);
            if (ValidationModel.Role.Equals(Role.Admin))
            {
                userList = _userApiService.GetAllUsers();
                return View(userList);
            }

            if (ValidationModel.Role.Equals(Role.Customer))
            {
                var user = _userApiService.GetUser(ValidationModel.Id);
                userList = new List<User>() { user };
                return View(userList);
            }
        }

        return RedirectToAction("Forbidden");
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            _userApiService.PostUser(user);
            TempData["success"] = "User created successfully";

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        return View(user);
    }

    public IActionResult Edit(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        if (ValidationModel.IsCredentialsMatched)
        {
            if (ValidationModel.Role.Equals(Role.Customer) && ValidationModel.Id != id)
                return RedirectToAction("Forbidden");

            _userApiService.AddTokenToHeader(ValidationModel.Token);
            var userFromDb = _userApiService.GetUser(id);
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
    public IActionResult Edit(User user)
    {
        if (ValidationModel.IsCredentialsMatched && ModelState.IsValid)
        {
            _userApiService.AddTokenToHeader(ValidationModel.Token);
            _userApiService.UpdateUser(user);
            TempData["success"] = "User updated successfully";
            return RedirectToAction("Index");
        }

        return View(user);
    }

    public IActionResult Delete(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        if (ValidationModel.IsCredentialsMatched)
        {
            if (ValidationModel.Role.Equals(Role.Customer) && ValidationModel.Id != id)
                return RedirectToAction("Forbidden");

            _userApiService.AddTokenToHeader(ValidationModel.Token);
            var userFromDb = _userApiService.GetUser(id);
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
    public IActionResult DeletePost(int id)
    {
        _userApiService.AddTokenToHeader(ValidationModel.Token);
        var userFromDb = _userApiService.GetUser(id);
        if (userFromDb == null)
        {
            return NotFound();
        }

        _userApiService.DeleteUser(id);
        TempData["success"] = "User deleted successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Forbidden()
    {
        return View();
    }
}