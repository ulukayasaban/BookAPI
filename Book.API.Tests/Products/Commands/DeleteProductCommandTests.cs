using Book.API.Application.Products.Commands;
using Book.API.Application.Products.Handlers;

namespace Book.API.Tests.Products.Commands
{
    public class DeleteProductCommandTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<DeleteProductHandler>> _mockLogger;

        public DeleteProductCommandTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<DeleteProductHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenExists()
        {
            // Arrange
            var product = new Product { ProductId = 1, ProductName = "To Be Deleted" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var handler = new DeleteProductHandler(_mockRepo.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteProductCommand(1), default);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.Delete(product), Times.Once);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

            var handler = new DeleteProductHandler(_mockRepo.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteProductCommand(99), default);

            // Assert
            result.Should().BeFalse();
            _mockRepo.Verify(r => r.Delete(It.IsAny<Product>()), Times.Never);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}