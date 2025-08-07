using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace backend.Domains.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IDistributedCache _cache;

        public CategoryRepository(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var cachedCategories = await _cache.GetStringAsync("categories");
            
            if (string.IsNullOrEmpty(cachedCategories))
            {

                var categories = await _context.Set<Category>()
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();
                _cache.SetString("categories", JsonSerializer.Serialize(categories));
            }
            return JsonSerializer.Deserialize<IEnumerable<Category>>(_cache.GetString("categories"));
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Set<Category>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the category by ID.", ex);
            }
        }

        public async Task<Category> AddAsync(Category category)
        {
            try
            {
                category.CreatedAt = DateTime.UtcNow;
                category.UpdatedAt = DateTime.UtcNow;
                var _category = await _context.Set<Category>().AddAsync(category);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync("categories");
                await _cache.RemoveAsync("products");

                return _category.Entity;
            }          
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category.", ex);
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            try
            {
                category.UpdatedAt = DateTime.UtcNow;
                var _category = _context.Set<Category>().Update(category);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync("categories");
                await _cache.RemoveAsync("products");

                return _category.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        public async Task<Category> DeleteAsync(Guid id)
        {
            try
            {
                var category = await GetByIdAsync(id);

                if (category == null)
                {
                    throw new Exception("Category not found.");
                }
  
                _context.Set<Category>().Remove(category);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync("categories");
                await _cache.RemoveAsync("products");
                return category;
                
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }

    }
}
