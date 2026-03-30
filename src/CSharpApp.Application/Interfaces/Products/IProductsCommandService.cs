using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Application.Interfaces.Products
{
    public interface IProductsCommandService
    {
        Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    }
}
