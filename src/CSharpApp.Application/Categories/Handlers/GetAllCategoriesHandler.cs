using CSharpApp.Application.Categories.Queries;
using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using MediatR;

namespace CSharpApp.Application.Categories.Handlers;

public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyCollection<Category>>
{
    private readonly ICategoriesService _categoriesService;

    public GetAllCategoriesHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public Task<IReadOnlyCollection<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return _categoriesService.GetAllAsync(cancellationToken);
    }
        
}

