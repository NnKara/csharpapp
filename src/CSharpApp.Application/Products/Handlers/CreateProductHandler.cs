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
        return _productsService.CreateAsync(request.Request, cancellationToken);
    }
}

