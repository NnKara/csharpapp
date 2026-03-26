namespace CSharpApp.Application.Interfaces.Products;

using CSharpApp.Core.Dtos.Product;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
}
