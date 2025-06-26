using Book.API.Application.Categories.Commands;
using Book.API.Application.Categories.Handlers;

namespace Book.API.Tests.Categories.Commands
{
    public class UpdateCategoryCommandTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly Mock<ILogger<UpdateCategoryHandler>> _mockLogger;

        public UpdateCategoryCommandTests()
        {
            _mapper = MapperHelper.CreateMapper();
            _mockRepo = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<UpdateCategoryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldUpdateCategory_WhenExists()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Old" };
            var dto = new CategoryDto { Id = 1, Name = "Updated" };

            _mockRepo.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existingCategory);

            var handler = new UpdateCategoryHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(dto), default);

            // Assert
            result.Should().BeTrue();
            _mockRepo.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCategoryNotFound()
        {
            // Arrange
            var dto = new CategoryDto { Id = 99, Name = "NonExisting" };
            _mockRepo.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((Category?)null);

            var handler = new UpdateCategoryHandler(_mockRepo.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new UpdateCategoryCommand(dto), default);

            // Assert
            result.Should().BeFalse();
            _mockRepo.Verify(r => r.Update(It.IsAny<Category>()), Times.Never);
            _mockRepo.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}