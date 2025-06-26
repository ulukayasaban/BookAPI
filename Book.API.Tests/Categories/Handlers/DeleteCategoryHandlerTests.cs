using Book.API.Application.Categories.Commands;
using Book.API.Application.Categories.Handlers;

namespace Book.API.Tests.Categories.Handlers
{
    public class DeleteCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly Mock<ILogger<DeleteCategoryHandler>> _mockLogger;

    public DeleteCategoryHandlerTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mockLogger = new Mock<ILogger<DeleteCategoryHandler>>();
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenCategoryExists()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Silinecek" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

        var handler = new DeleteCategoryHandler(_mockRepo.Object, _mockLogger.Object);
        var command = new DeleteCategoryCommand(1);

        // Act
        var result = await handler.Handle(command, default);

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
        var command = new DeleteCategoryCommand(99);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Delete(It.IsAny<Category>()), Times.Never);
    }
}
}