using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Infrastructure.Categories;
using CSharpApp.Infrastructure.JwtAuth;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        var httpClientSettings = configuration.GetSection(nameof(HttpClientSettings))
                                 .Get<HttpClientSettings>() ?? new HttpClientSettings();     

        var handlerLifetime = TimeSpan.FromMinutes(httpClientSettings.LifeTime);

        services.AddHttpClient(ExternalApiHttpClients.AuthOnly, (sp, client) =>
        {
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;

            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpClientSettings.TimeoutSeconds);
        })
        .SetHandlerLifetime(handlerLifetime);

        services.AddSingleton<IAuthTokenProvider, AuthTokenProvider>();
        services.AddTransient<BearerTokenHandler>();

        services.AddHttpClient<IProductsApiClient, ProductsApiClient>((sp, client) =>
        {
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;

            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpClientSettings.TimeoutSeconds);
        })
        .AddHttpMessageHandler<BearerTokenHandler>()
        .SetHandlerLifetime(handlerLifetime)
        .AddPolicyHandler((_, _) =>
            HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(
                retryCount: httpClientSettings.RetryCount,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromMilliseconds(httpClientSettings.SleepDuration * attempt)));

        services.AddHttpClient<ICategoriesApiClient, CategoriesApiClient>((sp, client) =>
        {
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;

            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpClientSettings.TimeoutSeconds);
        })
        .AddHttpMessageHandler<BearerTokenHandler>()
        .SetHandlerLifetime(handlerLifetime)
        .AddPolicyHandler((_, _) =>
            HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(
                retryCount: httpClientSettings.RetryCount,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromMilliseconds(httpClientSettings.SleepDuration * attempt)));

        return services;
    }

}