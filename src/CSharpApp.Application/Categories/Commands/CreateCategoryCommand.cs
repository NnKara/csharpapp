using CSharpApp.Core.Dtos.Category;
using MediatR;

namespace CSharpApp.Application.Categories.Commands;

public sealed record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<Category>;

