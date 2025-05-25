using System.Text.Json;
using backend.Shared;
using Domains.Categories;
using Microsoft.Extensions.Caching.Distributed;

namespace backend.Domains.Categories;

class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDtoOut>> GetAllAsync()
    {
        return (await _categoryRepository.GetAllAsync()).ToList().Select(category => MapToDto(category));
    }

    public async Task<CategoryDtoOut> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new Exception("Category not found.");
        }

        return MapToDto(category);
    }

    public async Task<CategoryDtoOut> AddAsync(CategoryDtoIn categoryDto)
    {
        try
        {
            var category = MapToEntity(categoryDto);
            var categoryEntity = await _categoryRepository.AddAsync(category);
          
            if (categoryEntity == null)
            {
                throw new Exception("Failed to add the category.");
            }

            return MapToDto(categoryEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the category: {ex.Message}");
        }
    }

    public async Task<CategoryDtoOut> UpdateAsync(CategoryDtoIn categoryDto, Guid id)
    {
        try
        {
            var category = MapToEntity(categoryDto);
            category.Id = id;
            var categoryEntity = await _categoryRepository.UpdateAsync(category);
           
            if (categoryEntity == null)
            {
                throw new Exception("Category not found.");
            }

            return MapToDto(categoryEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the category: {ex.Message}");
        }
    }

    public async Task<CategoryDtoOut> DeleteAsync(Guid id)
    {
        try
        {
            var categoryEntity = await _categoryRepository.DeleteAsync(id);
            
            if (categoryEntity == null)
            {
                throw new Exception("Category not found.");
            }

            // await _cache.RemoveAsync("categories");
            return MapToDto(categoryEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the category: {ex.Message}");
        }
    }

    private CategoryDtoOut MapToDto(Category category)
    {
        return new CategoryDtoOut
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    private Category MapToEntity(CategoryDtoIn categoryDto)
    {
        return new Category
        {
            Id = categoryDto.Id ?? Guid.Empty,
            Name = categoryDto.Name
        };
    }
}
