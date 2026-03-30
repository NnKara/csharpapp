using CSharpApp.Core.Dtos.Category;

namespace CSharpApp.Application.Interfaces.Categories
{
    public interface ICategoriesCommandService
    {
        Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    }
}
