using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Application.Products.Queries;
using CSharpApp.Core.Dtos.Product;
using MediatR;

namespace CSharpApp.Application.Products.Handlers;

public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductsService _productsService;

    public GetProductByIdHandler(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return _productsService.GetByIdAsync(request.Id, cancellationToken);
    }
        
}

