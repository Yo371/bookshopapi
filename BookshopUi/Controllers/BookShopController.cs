using BookshopUi.Services;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookshopUi.Controllers;

public class BookShopController : Controller
{
    protected ValidationModel ValidationModel => _validationService.GetValidationModel(HttpContext);

    protected bool IsCustomer => ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Customer);
    
    protected bool IsAdmin => ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Admin);
    
    protected bool IsManager => ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager);
    
    private readonly IValidationService _validationService;

    protected void SuccessNotification(string message) => TempData["success"] = message;
    

    public BookShopController(IValidationService validationService)
    {

        _validationService = validationService;
    }
}