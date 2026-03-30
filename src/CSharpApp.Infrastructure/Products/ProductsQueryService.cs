

using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Infrastructure.Products
{
    public sealed class ProductsQueryService : IProductsQueryService
    {

        private readonly IProductsService _productsService;


        public ProductsQueryService(IProductsService productsService)
        {
            _productsService = productsService;
        }


        public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _productsService.GetAllAsync(cancellationToken);
        }
           
        public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _productsService.GetByIdAsync(id, cancellationToken);
        }

    }
}
