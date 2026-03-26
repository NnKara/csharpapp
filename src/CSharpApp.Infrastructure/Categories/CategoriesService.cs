using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Infrastructure.Categories
{
    public sealed class CategoriesService : ICategoriesService
    {

        private readonly ICategoriesApiClient _categoriesApiClient;

        public CategoriesService(ICategoriesApiClient categoriesApiClient)
        {
            _categoriesApiClient = categoriesApiClient;
        }

        public Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _categoriesApiClient.GetAllAsync(cancellationToken);
        }

        public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _categoriesApiClient.GetByIdAsync(id, cancellationToken);
        }
    }
}
