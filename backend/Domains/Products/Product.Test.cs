using Xunit;
using backend.Domains.Products;
using backend.Domains.Categories;
using backend.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ProductTests
{
    [Fact]
    public async Task ShouldAddProductToDatabase()
    {
        var context = TestObjectsFactory.CreateContext();
        var distributedCache = TestObjectsFactory.CreateCache();

        // Create and add a category first, since Product requires a CategoryId
        var categoryRepository = new CategoryRepository(context, distributedCache);
        var categoryService = new CategoryService(categoryRepository);
        var category = await categoryService.AddAsync(new Domains.Categories.CategoryDtoIn { Name = "Test Category" });

        var productRepository = new ProductRepository(context, distributedCache, categoryRepository);
        var productService = new ProductService(productRepository);

        var products = await productService.GetAllAsync();
        var count = products.Count();
        Assert.Equal(expected: 0, actual: count);

        await productService.AddAsync(new Domains.Products.ProductDtoIn
        {
            Name = "Test Product",
            Description = "Test Description",
            CategoryId = category.Id
        });

        products = await productService.GetAllAsync();
        count = products.Count();
        Assert.Equal(expected: 1, actual: count);
    }
}
