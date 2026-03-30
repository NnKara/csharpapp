using CSharpApp.Application.Helpers;
using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Application.Products.Commands;
using CSharpApp.Core.Dtos.Product;
using MediatR;

namespace CSharpApp.Application.Products.Handlers;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IProductsService _productsService;

    public CreateProductHandler(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var errors = CreateProductRequestValidator.ValidateCreateProdReq(request.Request);

        if (errors.Count > 0)
            throw new ArgumentException(string.Join("; ", errors));

        return _productsService.CreateAsync(request.Request, cancellationToken);
    }
}

