using CSharpApp.Infrastructure.JwtAuth;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var handlerLifetime = TimeSpan.FromMinutes(10);

        services.AddHttpClient(ExternalApiHttpClients.AuthOnly,(sp, client) =>
        {
            var httpSettings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;

            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpSettings.TimeoutSeconds);
        })
        .SetHandlerLifetime(handlerLifetime);

        services.AddSingleton<IAuthTokenProvider, AuthTokenProvider>();
        services.AddTransient<BearerTokenHandler>();

        services.AddHttpClient<IProductsApiClient, ProductsApiClient>((sp, client) =>
        {
            var httpSettings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;

            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpSettings.TimeoutSeconds);
        })
        .AddHttpMessageHandler<BearerTokenHandler>()
        .SetHandlerLifetime(handlerLifetime)
        .AddPolicyHandler((sp, _) =>
        {
            var inner = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;

            return HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(
                retryCount: inner.RetryCount,
                sleepDurationProvider: attempt =>
                TimeSpan.FromMilliseconds(inner.SleepDuration * attempt));
        });

        return services;
    }
}