namespace CSharpApp.Infrastructure.Products;
using CSharpApp.Core.Dtos;

public class ProductsService : IProductsService
{
    private readonly IProductsApiClient _productsApiClient;
    public ProductsService(IProductsApiClient productsApiClient)
    {
        _productsApiClient = productsApiClient;
    }

    public Task<IReadOnlyCollection<Product>> GetProducts()
    {
       return _productsApiClient.GetAllAsync();
    }

    public Task<Product?> GetProduct(int id, CancellationToken cancellationToken = default)
    {
        return _productsApiClient.GetOneAsync(id, cancellationToken);
    }
}
