using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Core.Dtos.Category;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

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

            var categories = await response.Content.ReadFromJsonAsync<List<Category>>(cancellationToken) ?? [];

            return categories.AsReadOnly();
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"{_restApiSettings.Categories}/{id}";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Category>(cancellationToken);
        }

        public async Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Categories, request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var createdCategory = await response.Content.ReadFromJsonAsync<Category>(cancellationToken);

            if (createdCategory is null)
                throw new InvalidOperationException("Category response could not be parsed.");

            return createdCategory;
        }
    }
}
