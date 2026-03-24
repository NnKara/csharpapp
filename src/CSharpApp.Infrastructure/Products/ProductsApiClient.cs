using CSharpApp.Core.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
    }
}
