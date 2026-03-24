namespace CSharpApp.Application.Interfaces;

using CSharpApp.Core.Dtos;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProducts();
}
