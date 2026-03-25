using CSharpApp.Infrastructure.JwtAuth;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var httpSettings = serviceProvider.GetRequiredService<IOptions<HttpClientSettings>>().Value;
        var restApiSettings = serviceProvider.GetRequiredService<IOptions<RestApiSettings>>().Value;

        services.AddHttpClient(ExternalApiHttpClients.AuthOnly,(_, client) =>
        {
            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpSettings.TimeoutSeconds);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpSettings.LifeTime));

        services.AddSingleton<IAuthTokenProvider, AuthTokenProvider>();
        services.AddTransient<BearerTokenHandler>();

        services.AddHttpClient<IProductsApiClient, ProductsApiClient>((_, client) =>
        {
            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpSettings.TimeoutSeconds);
        })
        .AddHttpMessageHandler<BearerTokenHandler>()
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpSettings.LifeTime))
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