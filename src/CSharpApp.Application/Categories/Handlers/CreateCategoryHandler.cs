using CSharpApp.Application.Categories.Commands;
using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using MediatR;

namespace CSharpApp.Application.Categories.Handlers;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoriesService _categoriesService;

    public CreateCategoryHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return _categoriesService.CreateAsync(request.Request, cancellationToken);
    }
}

