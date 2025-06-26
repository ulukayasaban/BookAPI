using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book.API.Tests.Repositories
{
    public class RepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var context = InMemoryDbHelper.CreateDbContext();
            var repo = new Repository<Category>(context);
            var category = new Category { Name = "Test Kategori" };

            // Act
            await repo.AddAsync(category);
            await repo.SaveAsync();

            // Assert
            var result = await repo.GetAllAsync();
            result.Should().ContainSingle().Which.Name.Should().Be("Test Kategori");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var context = InMemoryDbHelper.CreateDbContext();
            var repo = new Repository<Product>(context);
            var product = new Product { ProductName = "Test", Price = 10, CategoryId = 1 };

            await repo.AddAsync(product);
            await repo.SaveAsync();

            var result = await repo.GetByIdAsync(product.ProductId);
            result.Should().NotBeNull();
            result!.ProductName.Should().Be("Test");
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            var context = InMemoryDbHelper.CreateDbContext();
            var repo = new Repository<Category>(context);
            var category = new Category { Name = "Silinecek" };

            await repo.AddAsync(category);
            await repo.SaveAsync();

            repo.Delete(category);
            await repo.SaveAsync();

            var result = await repo.GetAllAsync();
            result.Should().BeEmpty();
       } 
    } 
}