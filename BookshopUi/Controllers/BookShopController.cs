using BookshopUi.Services;
using Commons.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookshopUi.Controllers;

public class BookShopController : Controller
{
    protected ValidationModel ValidationModel => _validationService.GetValidationModel(HttpContext);
    
    private readonly IValidationService _validationService;
    

    public BookShopController(IValidationService validationService)
    {

        _validationService = validationService;
    }
}