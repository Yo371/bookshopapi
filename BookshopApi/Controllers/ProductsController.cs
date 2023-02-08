using BookshopApi.Models;
using BookshopApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookshopApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Customer")]
        [Route("api/products/")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Customer")]
        [Route("api/products/{id}")]
        public Product GetProduct(int id)
        {
            return _productService.GetProduct(id);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("api/products")]
        public void UpdateProduct([FromBody]Product product)
        {
            _productService.UpdateProduct(product);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("api/products")]
        public Product CreateProduct([FromBody]Product product)
        {
            _productService.CreateProduct(product);

            return product;
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("api/products/{id:int}")]
        public void DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
        }
    }
}
