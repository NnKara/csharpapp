using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Product;
using CSharpApp.Infrastructure.Products;

namespace CSharpApp.Tests.ProductTest
{
    public class ProductsServiceTests
    {

        [Fact]
        public async Task GetAllProducts()
        {
            var expectedProducts = new List<Product>();
            var mockClient = new MockProductsApiClient { GetAllResult = expectedProducts };
            var service = new ProductsService(mockClient);
            var result = await service.GetAllAsync();
            Assert.Same(expectedProducts, result);
        }


        [Fact]
        public async Task GetProductById()
        {
            var expectedProduct = new Product { Id = 5, Title = "Book" };
            var mockClient = new MockProductsApiClient { GetByIdResult = expectedProduct };
            var service = new ProductsService(mockClient);
            var result = await service.GetByIdAsync(5);
            Assert.Same(expectedProduct, result);
        }


        [Fact]
        public async Task CreateProduct()
        {

            var request = new CreateProductRequest
            {
                Title = "New Product",
                Price = 10,
                Description = "A product",
                CategoryId = 1
            };

            var expectedProduct = new Product { Id = 100, Title = "New Product" };
            var mockClient = new MockProductsApiClient { CreateResult = expectedProduct };
            var service = new ProductsService(mockClient);
            var result = await service.CreateAsync(request);
            Assert.Same(expectedProduct, result);
        }




        private sealed class MockProductsApiClient : IProductsApiClient
        {
            public IReadOnlyCollection<Product> GetAllResult { get; set; } = Array.Empty<Product>();
            public Product? GetByIdResult { get; set; }
            public Product CreateResult { get; set; } = null!;


            public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default) 
                => Task.FromResult(GetAllResult);
            public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
                => Task.FromResult(GetByIdResult);
            public Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
                => Task.FromResult(CreateResult);
        }
    }
}
