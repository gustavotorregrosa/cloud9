using backend.Shared;
using Domains.Products;

namespace backend.Domains.Products
{
    public interface IProductService : IService<ProductDtoIn, ProductDtoOut>
    {
       
    }
}
