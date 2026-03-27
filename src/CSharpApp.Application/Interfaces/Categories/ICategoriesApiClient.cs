using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Application.Interfaces.Categories
{
    public interface ICategoriesApiClient
    {
        Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    }
}
