using Book.API.Application.Products.Commands;
using Book.API.Application.Products.Handlers;

namespace Book.API.Tests.Products.Commands
{
    public class UpdateProductCommandTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<UpdateProductHandler>> _mockLogger;

        public UpdateProductCommandTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<UpdateProductHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenExists()
        {
            // Arrange
            var existingProduct = new Product { ProductId = 1, ProductName = "Old", Price = 5, CategoryId = 1 };
            var dto = new ProductDto { ProductId = 1, ProductName = "Updated", Price = 15, CategoryId = 1 };

            _mockRepo.Setup(r => r.GetByIdAsync(dto.ProductId)).ReturnsAsync(existingProduct);

            var handler = new UpdateProductHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new UpdateProductCommand(dto), default);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductNotFound()
        {
            // Arrange
            var dto = new ProductDto { ProductId = 99, ProductName = "Doesn't Exist", Price = 50, CategoryId = 1 };

            _mockRepo.Setup(r => r.GetByIdAsync(dto.ProductId)).ReturnsAsync((Product?)null);

            var handler = new UpdateProductHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new UpdateProductCommand(dto), default);

            // Assert
            result.Should().BeFalse();
            _mockRepo.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}