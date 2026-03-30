using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Application.Interfaces.Products
{
    public interface IProductsQueryService
    {
        Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
