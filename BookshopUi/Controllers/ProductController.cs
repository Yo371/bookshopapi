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

        public IActionResult Index()
        {
            if (ValidationModel.IsCredentialsMatched &&
                (ValidationModel.Role.Equals(Role.Manager) || ValidationModel.Role.Equals(Role.Customer)))
            {
                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productsList = _productApiService.GetAllProducts();
                return View(productsList);
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
        public IActionResult Create(Product product)
        {
            if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
            {
                if (ModelState.IsValid)
                {
                    _productApiService.AddTokenToHeader(ValidationModel.Token);
                    _productApiService.PostProduct(product);
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }

                return View(product);
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

                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productFromDb = _productApiService.GetProduct(id);
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
        public IActionResult Edit(Product product)
        {
            if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
            {
                if (ModelState.IsValid)
                {
                    _productApiService.AddTokenToHeader(ValidationModel.Token);
                    _productApiService.UpdateProduct(product);
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }

                return View(product);
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

                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productFromDb = _productApiService.GetProduct(id);
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
        public IActionResult DeletePost(int id)
        {
            if (ValidationModel.IsCredentialsMatched && ValidationModel.Role.Equals(Role.Manager))
            {
                _productApiService.AddTokenToHeader(ValidationModel.Token);
                var productFromDb = _productApiService.GetProduct(id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                
                _productApiService.DeleteProduct(id);
                TempData["success"] = "Product delete successfully";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Forbidden", "User");
        }
    }
}