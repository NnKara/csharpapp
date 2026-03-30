using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Application.Products.Queries;
using CSharpApp.Core.Dtos.Product;
using MediatR;

namespace CSharpApp.Application.Products.Handlers;

public sealed class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<Product>>
{
    private readonly IProductsService _productsService;

    public GetAllProductsHandler(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public Task<IReadOnlyCollection<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
       return _productsService.GetAllAsync(cancellationToken);
    }
        
}

