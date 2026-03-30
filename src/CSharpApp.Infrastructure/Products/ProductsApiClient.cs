using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Product;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace CSharpApp.Infrastructure.Products
{
    public sealed class ProductsApiClient : IProductsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        public ProductsApiClient(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings)
        {
            _httpClient = httpClient;
            _restApiSettings = restApiSettings.Value;
        }
        public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Products, cancellationToken);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync<List<Product>>(cancellationToken) ?? [];
            return products.AsReadOnly();
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"{_restApiSettings.Products}/{id}";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>(cancellationToken);
        }


        public async Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Products, request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var createdProduct = await response.Content.ReadFromJsonAsync<Product>(cancellationToken);

            if (createdProduct is null)
                throw new InvalidOperationException("Product response could not be parsed.");

            return createdProduct;
        }
    }
}
