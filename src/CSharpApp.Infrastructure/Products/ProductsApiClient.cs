using CSharpApp.Core.Dtos;
using Microsoft.Extensions.Options;
using System.Net;
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

        public async Task<Product?> GetOneAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"{_restApiSettings.Products}/{id}";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<Product>(content);
        }
    }
}
