using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Application.Interfaces.Categories
{
    public interface ICategoriesApiClient
    {
        Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
