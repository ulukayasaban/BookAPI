using Book.API.Application.Categories.Handlers;
using Book.API.Application.Categories.Queries;

namespace Book.API.Tests.Categories.Queries
{
    public class GetCategoryByIdQueryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly Mock<ILogger<GetCategoryByIdHandler>> _mockLogger;

        public GetCategoryByIdQueryTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<GetCategoryByIdHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnCategory_WhenExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            var handler = new GetCategoryByIdHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetCategoryByIdQuery(1), default);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Test");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            var handler = new GetCategoryByIdHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetCategoryByIdQuery(99), default);

            // Assert
            result.Should().BeNull();
        }
    }
}