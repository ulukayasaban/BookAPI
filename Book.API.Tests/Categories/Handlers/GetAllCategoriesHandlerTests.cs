using Book.API.Application.Categories.Handlers;
using Book.API.Application.Categories.Queries;

namespace Book.API.Tests.Categories.Handlers
{
    public class GetAllCategoriesHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetAllCategoriesHandler>> _mockLogger;

        public GetAllCategoriesHandlerTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _mapper = MapperHelper.CreateMapper();
            _mockLogger = new Mock<ILogger<GetAllCategoriesHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCategories()
        {
            // Arrange
            var fakeCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Kategori 1" },
                new Category { Id = 2, Name = "Kategori 2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeCategories);

            var handler = new GetAllCategoriesHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllCategoriesQuery(), default);

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Kategori 1");
        }
    }
}