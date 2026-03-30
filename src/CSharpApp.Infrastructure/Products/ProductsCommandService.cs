

using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Infrastructure.Products
{
    public sealed class ProductsCommandService : IProductsCommandService
    {

        private readonly IProductsService _productsService;


        public ProductsCommandService(IProductsService productsService)
        {
            _productsService = productsService;
        }


        public Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            return _productsService.CreateAsync(request, cancellationToken);
        }

    }
}
