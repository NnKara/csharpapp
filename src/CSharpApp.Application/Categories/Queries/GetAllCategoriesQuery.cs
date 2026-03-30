using CSharpApp.Core.Dtos.Category;
using MediatR;

namespace CSharpApp.Application.Categories.Queries;

public sealed record GetAllCategoriesQuery : IRequest<IReadOnlyCollection<Category>>;

