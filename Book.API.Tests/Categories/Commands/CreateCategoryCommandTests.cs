using Book.API.Application.Categories.Commands;
using Book.API.Application.Categories.Handlers;

namespace Book.API.Tests.Categories.Commands
{
    public class CreateCategoryCommandTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly Mock<ILogger<CreateCategoryHandler>> _mockLogger;

        public CreateCategoryCommandTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<CreateCategoryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldCreateCategory()
        {
            // Arrange
            var dto = new CategoryDto { Id = 5, Name = "Test" };
            var handler = new CreateCategoryHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreateCategoryCommand(dto), default);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test");
            _mockRepo.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
            _mockRepo.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}