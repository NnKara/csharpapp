namespace CSharpApp.Infrastructure.Products;

using System.Text.Json;
using CSharpApp.Core.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
}
