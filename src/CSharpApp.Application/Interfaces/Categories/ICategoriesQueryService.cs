using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Application.Interfaces.Categories
{
    public interface ICategoriesQueryService
    {
        Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
