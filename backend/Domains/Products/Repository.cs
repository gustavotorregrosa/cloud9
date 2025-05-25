using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Domains.Categories;
using backend.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace backend.Domains.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IDistributedCache _cache;
        
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(ApplicationDbContext context, IDistributedCache cache, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {

            var categories = await _categoryRepository.GetAllAsync();
            var cachedProducts = await _cache.GetStringAsync("products");
            if (string.IsNullOrEmpty(cachedProducts))
            {
                var products = await _context.Set<Product>()
                    .OrderBy(p => p.CreatedAt)
                    .ToListAsync();
                foreach (var product in products)
                {
                    var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
                    if (category != null)
                    {
                        product.Category = new Category
                        {
                            Id = category.Id,
                            Name = category.Name

                        };
                    }
                }
                
                _cache.SetString("products", JsonSerializer.Serialize(products));
            }
            return JsonSerializer.Deserialize<IEnumerable<Product>>(_cache.GetString("products"));
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Set<Product>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the product by ID.", ex);
            }
        }

        public async Task<Product> AddAsync(Product product)
        {
            try
            {
                product.CreatedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;
                var _product = await _context.Set<Product>().AddAsync(product);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync("products");
                await _cache.RemoveAsync("categories");

                return _product.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            try
            {
                product.UpdatedAt = DateTime.UtcNow;
                var _product = _context.Set<Product>().Update(product);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync("products");
                await _cache.RemoveAsync("categories");
                return _product.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        public async Task<Product> DeleteAsync(Guid id)
        {
            try
            {
                var product = await GetByIdAsync(id);

                if (product == null)
                {
                    throw new Exception("Product not found.");
                }
  
                _context.Set<Product>().Remove(product);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync("categories");
                await _cache.RemoveAsync("products");
                
                return product;
                
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product.", ex);
            }
        }
    }
}
