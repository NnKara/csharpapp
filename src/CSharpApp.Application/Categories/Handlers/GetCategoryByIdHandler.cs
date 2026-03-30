using CSharpApp.Application.Categories.Queries;
using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using MediatR;

namespace CSharpApp.Application.Categories.Handlers;

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly ICategoriesService _categoriesService;

    public GetCategoryByIdHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return _categoriesService.GetByIdAsync(request.Id, cancellationToken);
    }
        
}

