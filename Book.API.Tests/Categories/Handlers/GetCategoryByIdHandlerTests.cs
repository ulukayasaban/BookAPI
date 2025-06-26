using Book.API.Application.Categories.Handlers;
using Book.API.Application.Categories.Queries;

namespace Book.API.Tests.Categories.Handlers
{
    public class GetCategoryByIdHandlerTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<GetCategoryByIdHandler>> _mockLogger;

    public GetCategoryByIdHandlerTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mapper = MapperHelper.CreateMapper();
        _mockLogger = new Mock<ILogger<GetCategoryByIdHandler>>();
    }

    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenExists()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Kitap" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

        var handler = new GetCategoryByIdHandler(_mockRepo.Object, _mapper, _mockLogger.Object);
        var query = new GetCategoryByIdQuery(1);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Kitap");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        var handler = new GetCategoryByIdHandler(_mockRepo.Object, _mapper, _mockLogger.Object);
        var query = new GetCategoryByIdQuery(99);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().BeNull();
    }
}
}