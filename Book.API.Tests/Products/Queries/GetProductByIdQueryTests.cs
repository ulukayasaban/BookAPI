using Book.API.Application.Products.Handlers;
using Book.API.Application.Products.Queries;

namespace Book.API.Tests.Products.Queries
{
    public class GetProductByIdQueryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<GetProductByIdHandler>> _mockLogger;

        public GetProductByIdQueryTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<GetProductByIdHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var product = new Product { ProductId = 1, ProductName = "Test Ürün", Price = 55.5m, CategoryId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var handler = new GetProductByIdHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(1), default);

            // Assert
            result.Should().NotBeNull();
            result!.ProductId.Should().Be(1);
            result.ProductName.Should().Be("Test Ürün");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

            var handler = new GetProductByIdHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(99), default);

            // Assert
            result.Should().BeNull();
        }
    }
}