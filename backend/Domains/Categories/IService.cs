using backend.Domains;
using backend.Shared;
using Domains.Categories;

namespace backend.Domains.Categories
{
    public interface ICategoryService : IService<CategoryDtoIn, CategoryDtoOut>
    {
       
    }
}
