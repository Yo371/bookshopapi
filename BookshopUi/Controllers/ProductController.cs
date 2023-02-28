using BookshopUi.Services;
using Commons.Models;
using Commons.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace BookshopUi.Controllers
{
    public class ProductController : BookShopController
    {
        private readonly IProductApiService _productApiService;

        public ProductController(IProductApiService productApiService, IValidationService validationService) : base(
            validationService)
        {
            _productApiService = productApiService;
            _productApiService.SetClient(new RestClient(Constants.ApiUrl));
        }

        public async Task<IActionResult> Index()
        {
            if (IsManager || IsCustomer)
            {
                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productsList = await _productApiService.GetAllProducts();
                return View(productsList);
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
        public async Task<IActionResult> Create(Product product)
        {
            if (IsManager)
            {
                if (ModelState.IsValid)
                {
                    _productApiService.AddTokenToHeader(ValidationModel.Token);
                    await _productApiService.PostProduct(product);
                    SuccessNotification("Product created successfully");
                    return RedirectToAction(nameof(Index));
                }

                return View(product);
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

                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productFromDb = await _productApiService.GetProduct(id);
                if (productFromDb == null)
                {
                    return NotFound();
                }

                return View(productFromDb);
            }

            return RedirectToAction("Forbidden", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (IsManager)
            {
                if (ModelState.IsValid)
                {
                    _productApiService.AddTokenToHeader(ValidationModel.Token);
                    await _productApiService.UpdateProduct(product);
                    SuccessNotification("Product updated successfully");
                    return RedirectToAction(nameof(Index));
                }

                return View(product);
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

                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productFromDb = await _productApiService.GetProduct(id);
                if (productFromDb == null)
                {
                    return NotFound();
                }

                return View(productFromDb);
            }

            return RedirectToAction("Forbidden", "User");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (IsManager)
            {
                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productFromDb = await _productApiService.GetProduct(id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                
                await _productApiService.DeleteProduct(id);
                SuccessNotification("Product delete successfully");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Forbidden", "User");
        }
    }
}