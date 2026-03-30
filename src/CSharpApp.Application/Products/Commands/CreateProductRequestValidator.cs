using CSharpApp.Core.Dtos.Product;
using FluentValidation;

namespace CSharpApp.Application.Products.Commands
{
    public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");


            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");


            RuleFor(x => x.Images)
                .NotNull()
                .WithMessage("At least one image is required.")
                .Must(images => images is { Count: > 0 })
                .WithMessage("At least one image is required.");


            RuleForEach(x => x.Images!)
                .NotEmpty()
                .WithMessage("Images cannot contain empty values.");
        }
    }
}
