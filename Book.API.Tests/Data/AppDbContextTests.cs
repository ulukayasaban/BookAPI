using Book.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Tests.Data
{
    public class AppDbContextTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void DbSets_ShouldExist()
        {
            var context = GetInMemoryDbContext();
            context.Products.Should().NotBeNull();
            context.Categories.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Add_And_Get_Product()
        {
            var context = GetInMemoryDbContext();

            var product = new Product
            {
                ProductName = "Test Kitap",
                Price = 12.99m,
                CategoryId = 1
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var saved = await context.Products.FirstAsync();
            saved.ProductName.Should().Be("Test Kitap");
        }

        [Fact]
        public async Task Should_Seed_Data()
        {
            // EF Core InMemory Db seed veriyi otomatik yüklemez — manuel çağırmalıyız
            var context = GetInMemoryDbContext();
            await context.Database.EnsureCreatedAsync(); // Seed için önemli

            var categories = await context.Categories.ToListAsync();
            categories.Should().NotBeEmpty();
        }
    }
}