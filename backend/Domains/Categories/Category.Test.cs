using Xunit;
using backend.Domains.Categories;
using backend.Shared;

public class CategoryTests
{

    [Fact]
    public async Task ShouldAddCategoryToDatabase()
    {

        var context = TestObjectsFactory.CreateContext();
        var distributedCache = TestObjectsFactory.CreateCache();
        var repository = new CategoryRepository(context, distributedCache);
        var service = new CategoryService(repository);

        var _listaCategorias = await service.GetAllAsync();
        var count = _listaCategorias.Count();
        Assert.Equal(expected: 0, actual: count);
        
        service.AddAsync(new Domains.Categories.CategoryDtoIn { Name = "Test Category" }).Wait();

        _listaCategorias = await service.GetAllAsync();
        count = _listaCategorias.Count();
        Assert.Equal(expected: 1, actual: count);

    }

}