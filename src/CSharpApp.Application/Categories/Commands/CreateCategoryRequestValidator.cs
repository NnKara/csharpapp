using CSharpApp.Core.Dtos.Category;
using FluentValidation;

namespace CSharpApp.Application.Categories.Commands;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Image is required.");
    }
}

