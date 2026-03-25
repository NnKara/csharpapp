using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Application.Interfaces
{
    public interface IProductsApiClient
    {
        Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    }
}
