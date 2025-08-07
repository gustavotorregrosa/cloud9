using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace backend.Shared
{
    public class TestObjectsFactory
    {

        public static ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static IDistributedCache CreateCache()
        {
            var store = new Dictionary<string, byte[]>();
            var mockCache = new Mock<IDistributedCache>();

            mockCache.Setup(c => c.Get(It.IsAny<string>()))
                .Returns((string key) => store.TryGetValue(key, out var value) ? value : null);

            mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), default))
                .ReturnsAsync((string key, System.Threading.CancellationToken _) => store.TryGetValue(key, out var value) ? value : null);

            mockCache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()))
                .Callback((string key, byte[] value, DistributedCacheEntryOptions options) => store[key] = value);

            mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default))
                .Returns((string key, byte[] value, DistributedCacheEntryOptions options, System.Threading.CancellationToken _) =>
                {
                    store[key] = value;
                    return Task.CompletedTask;
                });

            mockCache.Setup(c => c.Remove(It.IsAny<string>()))
                .Callback((string key) => store.Remove(key));

            mockCache.Setup(c => c.RemoveAsync(It.IsAny<string>(), default))
                .Returns((string key, System.Threading.CancellationToken _) =>
                {
                    store.Remove(key);
                    return Task.CompletedTask;
                });

            return mockCache.Object;
        }

    }
}
