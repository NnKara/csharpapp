using CSharpApp.Core.Dtos.Product;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var products = JsonSerializer.Deserialize<List<Product>>(content) ?? [];
            return products.AsReadOnly();
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"{_restApiSettings.Products}/{id}";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<Product>(content);
        }


        public async Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Products, request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var productContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var createdProdcut = JsonSerializer.Deserialize<Product>(productContent);

            if (createdProdcut is null)
                throw new InvalidOperationException("Product response could not be parsed.");

            return createdProdcut;
        }
    }
}
