using Book.API.Application.Products.Handlers;
using Book.API.Application.Products.Queries;

namespace Book.API.Tests.Products.Queries
{
    public class GetAllProductsQueryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<GetAllProductsHandler>> _mockLogger;

        public GetAllProductsQueryTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<GetAllProductsHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Kitap 1", Price = 10, CategoryId = 1 },
                new Product { ProductId = 2, ProductName = "Kitap 2", Price = 20, CategoryId = 2 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeProducts);

            var handler = new GetAllProductsHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllProductsQuery(), default);

            // Assert
            result.Should().HaveCount(2);
            result[0].ProductName.Should().Be("Kitap 1");
            result[1].Price.Should().Be(20);
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}