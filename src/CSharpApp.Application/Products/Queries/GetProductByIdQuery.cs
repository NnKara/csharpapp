using CSharpApp.Core.Dtos.Product;
using MediatR;

namespace CSharpApp.Application.Products.Queries;

public sealed record GetProductByIdQuery(int Id) : IRequest<Product?>;

