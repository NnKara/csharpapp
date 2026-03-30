using CSharpApp.Core.Dtos.Product;
using MediatR;

namespace CSharpApp.Application.Products.Queries;

public sealed record GetAllProductsQuery : IRequest<IReadOnlyCollection<Product>>;

