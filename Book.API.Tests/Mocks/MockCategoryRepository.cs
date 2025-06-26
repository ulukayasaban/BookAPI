namespace Book.API.Tests.Mocks
{
    public static class MockCategoryRepository
    {
        public static Mock<ICategoryRepository> GetMock()
        {
            var mockRepo = new Mock<ICategoryRepository>();

            mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Category> {
                    new Category { Id = 1, Name = "Roman" },
                    new Category { Id = 2, Name = "Bilim" }
                });

            return mockRepo;
        }
    }
}