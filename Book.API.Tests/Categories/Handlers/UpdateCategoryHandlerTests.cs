using Book.API.Application.Categories.Commands;
using Book.API.Application.Categories.Handlers;

namespace Book.API.Tests.Categories.Handlers
{
    public class UpdateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UpdateCategoryHandler>> _mockLogger;

        public UpdateCategoryHandlerTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _mapper = MapperHelper.CreateMapper();
            _mockLogger = new Mock<ILogger<UpdateCategoryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldUpdate_WhenCategoryExists()
        {
            // Arrange
            var existing = new Category { Id = 1, Name = "Eski" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var dto = new CategoryDto { Id = 1, Name = "Yeni" };
            var handler = new UpdateCategoryHandler(_mockRepo.Object, _mapper, _mockLogger.Object);
            var command = new UpdateCategoryCommand(dto);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.Update(It.Is<Category>(c => c.Name == "Yeni")), Times.Once);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCategoryNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            var dto = new CategoryDto { Id = 99, Name = "Bilinmeyen" };
            var handler = new UpdateCategoryHandler(_mockRepo.Object, _mapper, _mockLogger.Object);
            var command = new UpdateCategoryCommand(dto);

            var result = await handler.Handle(command, default);

            result.Should().BeFalse();
            _mockRepo.Verify(r => r.Update(It.IsAny<Category>()), Times.Never);
        }
    }
}