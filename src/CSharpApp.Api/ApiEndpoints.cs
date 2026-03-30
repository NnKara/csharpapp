using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Application.Helpers;
using CSharpApp.Application.Products.Commands;
using CSharpApp.Application.Products.Queries;
using CSharpApp.Core.Dtos.Category;
using CSharpApp.Core.Dtos.Product;
using MediatR;

namespace CSharpApp.Api
{
    public static class ApiEndpoints
    {
        public static WebApplication RouteApiEndpoints(this WebApplication app)
        {
            var versioned = app.NewVersionedApi();


            versioned.MapGet("api/v{version:apiVersion}/getproducts", async (ISender mediator, CancellationToken ct) =>
            {
                var products = await mediator.Send(new GetAllProductsQuery(), ct);
                return Results.Ok(products);
            })
                .WithName("GetProducts")
                .HasApiVersion(1.0);


            versioned.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (int id, ISender mediator, CancellationToken ct) =>
            {
                var product = await mediator.Send(new GetProductByIdQuery(id), ct);
                if (product is null)
                    return Results.NotFound();
                return Results.Ok(product);
            })
                .WithName("GetProduct")
                .HasApiVersion(1.0);


            versioned.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductRequest request, ISender mediator, CancellationToken ct) =>
            {
                var created = await mediator.Send(new CreateProductCommand(request), ct);
                return Results.Ok(created);
            })
                .WithName("CreateProduct")
                .HasApiVersion(1.0);


            versioned.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesQueryService categoriesQueryService) =>
            {
                var categories = await categoriesQueryService.GetAllAsync();
                return Results.Ok(categories);
            })
                .WithName("GetCategories")
                .HasApiVersion(1.0);


            versioned.MapGet("api/v{version:apiVersion}/getcategory/{id:int}", async (int id, ICategoriesQueryService categoriesQueryService,
                    CancellationToken cancellationToken) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { message = "ID must be greater than 0" });
                var category = await categoriesQueryService.GetByIdAsync(id, cancellationToken);
                if (category is null)
                    return Results.NotFound();
                return Results.Ok(category);
            })
                .WithName("GetCategory")
                .HasApiVersion(1.0);


            versioned.MapPost("api/v{version:apiVersion}/createcategory", async (CreateCategoryRequest request,
                    ICategoriesCommandService categoriesCommandService,
                    CancellationToken ct) =>
            {
                var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(request);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });
                var created = await categoriesCommandService.CreateAsync(request, ct);
                return Results.Ok(created);
            })
                .WithName("CreateCategory")
                .HasApiVersion(1.0);
            return app;
        }
    }
}
