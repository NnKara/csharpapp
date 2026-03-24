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

        services.AddHttpClient<IProductsApiClient, ProductsApiClient>((sp, client) =>
        {
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;

            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpSettings.TimeoutSeconds);

        }).SetHandlerLifetime(TimeSpan.FromMinutes(httpSettings.LifeTime)).AddPolicyHandler((sp,_) =>

        { 
            var httpSettings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
            
            return HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(

                retryCount: httpSettings.RetryCount,
                sleepDurationProvider: attempt =>
                TimeSpan.FromMilliseconds(httpSettings.SleepDuration * attempt));
        });

        return services;
    }
}