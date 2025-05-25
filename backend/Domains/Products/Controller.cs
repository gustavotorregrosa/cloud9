using backend.Shared;
using Domains.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Domains.Products
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly WebSocketService _webSocketService;

        public ProductsController(IProductService productService, WebSocketService webSocketService)
        {
            _productService = productService;
            _webSocketService = webSocketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var user = User.Identity?.Name;
            if (user == null)
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            try
            {
                IEnumerable<ProductDtoOut> products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID: {id} not found." });
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDtoIn productDto)
        {
            var product = await _productService.AddAsync(productDto);
            _webSocketService.BroadcastMessage("refresh-products");
            if (product == null)
            {
                return BadRequest(new { Message = "Failed to create product." });
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDtoIn productDto)
        {
            var product = await _productService.UpdateAsync(productDto, id);
            _webSocketService.BroadcastMessage("refresh-products");
            return Ok(new { Message = $"Updated product with ID: {id}", Product = product });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteAsync(id);
            _webSocketService.BroadcastMessage("refresh-products");
            return Ok(new { Message = $"Deleted product with ID: {id}" });
        }
    }
}
