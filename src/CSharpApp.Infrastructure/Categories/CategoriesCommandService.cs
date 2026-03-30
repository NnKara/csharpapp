using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Infrastructure.Categories
{
    public sealed class CategoriesCommandService : ICategoriesCommandService
    {

        private readonly ICategoriesService _categoriesService;


        public CategoriesCommandService(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        public Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            return _categoriesService.CreateAsync(request, cancellationToken);
        }
    }
}
