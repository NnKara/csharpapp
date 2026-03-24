using Microsoft.Extensions.Options;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient<IProductsApiClient, ProductsApiClient>((sp, client) =>
        {
            var restApiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;
            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
        });
        return services;
    }
}