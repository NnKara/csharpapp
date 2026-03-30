using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Infrastructure.Categories
{
    public sealed class CategoriesQueryService : ICategoriesQueryService
    {

        private readonly ICategoriesService _categoriesService;


        public CategoriesQueryService(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _categoriesService.GetAllAsync(cancellationToken);
        }

        public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _categoriesService.GetByIdAsync(id, cancellationToken);
        }
    }
}
