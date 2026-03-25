using CSharpApp.Core.Dtos.JwtAuth;
using CSharpApp.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CSharpApp.Infrastructure.JwtAuth
{
    public sealed class AuthTokenProvider : IAuthTokenProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<RestApiSettings> _restApiSettings;
        private readonly ILogger<AuthTokenProvider> _logger;
        private readonly SemaphoreSlim _gate = new(1, 1);
        private string? _cachedToken;
        private DateTimeOffset _tokenExpiresAt = DateTimeOffset.MinValue;

        public AuthTokenProvider(IHttpClientFactory httpClientFactory,IOptions<RestApiSettings> restApiSettings,
                                 ILogger<AuthTokenProvider> logger)
        {
            _httpClientFactory = httpClientFactory;
            _restApiSettings = restApiSettings;
            _logger = logger;
        }
        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            await _gate.WaitAsync(cancellationToken);
            try
            {
                if (!string.IsNullOrEmpty(_cachedToken) && DateTimeOffset.UtcNow < _tokenExpiresAt)
                    return _cachedToken;

                var settings = _restApiSettings.Value;
                var client = _httpClientFactory.CreateClient(ExternalApiHttpClients.AuthOnly);

                var body = new LoginRequest
                {
                    Email = settings.Username ?? string.Empty,
                    Password = settings.Password ?? string.Empty
                };

                var response = await client.PostAsJsonAsync(settings.Auth, body, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("Login failed {Status}: {Body}", response.StatusCode, error);
                }

                response.EnsureSuccessStatusCode();

                var login = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);

                if (string.IsNullOrWhiteSpace(login?.AccessToken))
                    throw new InvalidOperationException("Login response did not include access_token.");

                _cachedToken = login.AccessToken;
                _tokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(50);

                return _cachedToken;
            }
            finally
            {
                _gate.Release();
            }
        }
    }
}
