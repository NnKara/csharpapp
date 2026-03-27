using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
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

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"{_restApiSettings.Categories}/{id}";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Categories, request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var categoryContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var createdCategory = JsonSerializer.Deserialize<Category>(categoryContent);

            if (createdCategory is null)
                throw new InvalidOperationException("Category response could not be parsed.");

            return createdCategory;
        }
    }
}
