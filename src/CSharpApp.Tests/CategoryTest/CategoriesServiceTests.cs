using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using CSharpApp.Infrastructure.Categories;

namespace CSharpApp.Tests.CategoryTest
{
    public class CategoriesServiceTests
    {

        [Fact]
        public async Task GetAllCategories()
        {
            var expectedCategories = new List<Category>();
            var mockClient = new MockCategoriesApiClient { GetAllResult = expectedCategories };
            var service = new CategoriesService(mockClient);
            var result = await service.GetAllAsync();
            Assert.Same(expectedCategories, result);
        }


        [Fact]
        public async Task GetCategoryById()
        {
            var expectedCategory = new Category { Id = 3, Name = "Electronics" };
            var mockClient = new MockCategoriesApiClient { GetByIdResult = expectedCategory };
            var service = new CategoriesService(mockClient);
            var result = await service.GetByIdAsync(3);
            Assert.Same(expectedCategory, result);
        }


        [Fact]
        public async Task CreateCategory()
        {
            var request = new CreateCategoryRequest
            {
                Name = "New Category",
                Image = "imageURL"
            };
            var expectedCategory = new Category { Id = 10, Name = "New Category" };
            var mockClient = new MockCategoriesApiClient { CreateResult = expectedCategory };
            var service = new CategoriesService(mockClient);
            var result = await service.CreateAsync(request);
            Assert.Same(expectedCategory, result);
        }


        private sealed class MockCategoriesApiClient : ICategoriesApiClient
        {
            public IReadOnlyCollection<Category> GetAllResult { get; set; } = Array.Empty<Category>();
            public Category? GetByIdResult { get; set; }
            public Category CreateResult { get; set; } = null!;


            public Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
                => Task.FromResult(GetAllResult);
            public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
                => Task.FromResult(GetByIdResult);
            public Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
                => Task.FromResult(CreateResult);
        }
    }
}
