using CSharpApp.Application.Categories.Commands;
using CSharpApp.Application.Helpers;
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
        var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(request.Request);

        if (errors.Count > 0)
            throw new ArgumentException(string.Join("; ", errors));

        return _categoriesService.CreateAsync(request.Request, cancellationToken);
    }
}

