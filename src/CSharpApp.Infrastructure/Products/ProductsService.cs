namespace CSharpApp.Infrastructure.Products;

using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Product;

public class ProductsService : IProductsService
{
    private readonly IProductsApiClient _productsApiClient;
    public ProductsService(IProductsApiClient productsApiClient)
    {
        _productsApiClient = productsApiClient;
    }

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
       return _productsApiClient.GetAllAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _productsApiClient.GetByIdAsync(id, cancellationToken);
    }

    public Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        return _productsApiClient.CreateAsync(request, cancellationToken);
    }

}
