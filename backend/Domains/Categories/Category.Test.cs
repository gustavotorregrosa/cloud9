using Xunit;
using Microsoft.EntityFrameworkCore;
using backend.Domains.Categories;


public class CategoryTests
{

    private DbContextOptions<ApplicationDbContext> CreateTestOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    private ApplicationDbContext CreateContext()
    {
        var options = CreateTestOptions();
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public void ShouldAddCategoryToDatabase()
    {


        using var context = CreateContext();

        // Set up a distributed cache (in-memory for testing)
        var cacheOptions = new Microsoft.Extensions.Options.OptionsWrapper<Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions>(
            new Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions());
        var distributedCache = new Microsoft.Extensions.Caching.Memory.MemoryDistributedCache(cacheOptions);

        var repository = new CategoryRepository(context, distributedCache);

        var service = new CategoryService(repository);

        // var config = context.AppConfigs.FirstOrDefault();
        // if (config == null)
        // {
        //     config = new AppConfig();
        // }


        // // Arrange
        // using var context = CreateContext();
        // var category = new CategoryDtoIn { Name = "Test Category" };

        // // Act
        // context.Categories.Add(new Category { Name = category.Name });
        // await context.SaveChangesAsync();

        // // Assert
        // var addedCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == category.Name);
        // Assert.NotNull(addedCategory);
        // Assert.Equal(category.Name, addedCategory.Name);
    }

}