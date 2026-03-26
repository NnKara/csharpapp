using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CSharpApp.Infrastructure.Categories
{
    public sealed class CategoriesApiClient : ICategoriesApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        public CategoriesApiClient(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings)
        {
            _httpClient = httpClient;
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Categories, cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var categories = JsonSerializer.Deserialize<List<Category>>(content) ?? [];

            return categories.AsReadOnly();
        }
    }
}
