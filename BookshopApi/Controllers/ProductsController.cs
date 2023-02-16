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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Product))]
        [Authorize(Roles = "Manager,Customer")]
        [Route("api/products/")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                return Ok(await _productService.GetAllProductsAsync());
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while getting the products.");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Product))]
        [Authorize(Roles = "Manager,Customer")]
        [Route("api/products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while getting the product.");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Product))]
        [Authorize(Roles = "Manager")]
        [Route("api/products")]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            try
            {
                await _productService.UpdateProductAsync(product);
                return StatusCode(StatusCodes.Status202Accepted, "The product updated.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the product.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Product))]
        [Authorize(Roles = "Manager")]
        [Route("api/products")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            try
            {
                await _productService.CreateProductAsync(product);

                return StatusCode(StatusCodes.Status201Created, product);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the product.");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Product))]
        [Authorize(Roles = "Manager")]
        [Route("api/products/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the product.");
            }
        }
    }
}