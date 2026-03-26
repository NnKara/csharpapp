using CSharpApp.Core.Dtos.Category;
using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Application.Interfaces.Categories
{
    public interface ICategoriesApiClient
    {
        Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
