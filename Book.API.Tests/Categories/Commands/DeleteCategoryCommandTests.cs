using Book.API.Application.Categories.Commands;
using Book.API.Application.Categories.Handlers;

namespace Book.API.Tests.Categories.Commands
{
    public class DeleteCategoryCommandTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly Mock<ILogger<DeleteCategoryHandler>> _mockLogger;

        public DeleteCategoryCommandTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<DeleteCategoryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteCategory_WhenExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "To Be Deleted" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            var handler = new DeleteCategoryHandler(_mockRepo.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteCategoryCommand(1), default);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.Delete(category), Times.Once);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCategoryNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            var handler = new DeleteCategoryHandler(_mockRepo.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteCategoryCommand(99), default);

            // Assert
            result.Should().BeFalse();
            _mockRepo.Verify(r => r.Delete(It.IsAny<Category>()), Times.Never);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}