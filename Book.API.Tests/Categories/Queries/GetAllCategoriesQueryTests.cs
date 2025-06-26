using Book.API.Application.Categories.Handlers;
using Book.API.Application.Categories.Queries;

namespace Book.API.Tests.Categories.Queries
{
    public class GetAllCategoriesQueryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly Mock<ILogger<GetAllCategoriesHandler>> _mockLogger;

        public GetAllCategoriesQueryTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<GetAllCategoriesHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCategories()
        {
            // Arrange
            var fakeData = new List<Category>
            {
                new Category { Id = 1, Name = "Roman" },
                new Category { Id = 2, Name = "Bilim" }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fakeData);

            var handler = new GetAllCategoriesHandler(
                _mockRepo.Object,
                _mapper,
                _mockLogger.Object
            );

            // Act
            var result = await handler.Handle(new GetAllCategoriesQuery(), default);

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Roman");
            result[1].Name.Should().Be("Bilim");
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}