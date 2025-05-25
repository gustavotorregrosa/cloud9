using backend.Shared;
using Domains.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Domains.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        private readonly WebSocketService _webSocketService;

        public CategoriesController(ICategoryService categoryService, WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {

            var user = User.Identity?.Name;
            if (user == null)
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            try
            {
                IEnumerable<CategoryDtoOut> categories = await _categoryService.GetAllAsync();
                return Ok(categories);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = $"Category with ID: {id} not found." });
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDtoIn categoryDto)
        {
            var category = await _categoryService.AddAsync(categoryDto);
            _webSocketService.BroadcastMessage("refresh-categories");
            if (category == null)
            {
                return BadRequest(new { Message = "Failed to create category." });
            }

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDtoIn categoryDto)
        {
            var category = await _categoryService.UpdateAsync(categoryDto, id);
            _webSocketService.BroadcastMessage("refresh-categories");
            return Ok(new { Message = $"Updated category with ID: {id}", Category = category });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            _webSocketService.BroadcastMessage("refresh-categories");
            return Ok(new { Message = $"Deleted category with ID: {id}" });
        }
    }
}
