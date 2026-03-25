using CSharpApp.Core.Dtos;

namespace CSharpApp.Application.Interfaces
{
    public interface IProductsApiClient
    {
        Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Product?> GetOneAsync(int id, CancellationToken cancellationToken = default);
    }
}
