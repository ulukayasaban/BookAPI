using Book.API.Application.Products.Commands;
using Book.API.Application.Products.Handlers;

namespace Book.API.Tests.Products.Commands
{
    public class CreateProductCommandTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<CreateProductHandler>> _mockLogger;

        public CreateProductCommandTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<CreateProductHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct()
        {
            // Arrange
            var dto = new ProductDto
            {
                ProductId = 1,
                ProductName = "Yeni Ürün",
                Price = 99.99m,
                CategoryId = 2
            };

            var handler = new CreateProductHandler(
                _mockRepo.Object,
                _mapper,
                _mockLogger.Object
            );

            // Act
            var result = await handler.Handle(new CreateProductCommand(dto), default);

            // Assert
            result.Should().NotBeNull();
            result.ProductName.Should().Be("Yeni Ürün");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}