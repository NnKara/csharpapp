using CSharpApp.Application.Categories.Commands;
using CSharpApp.Application.Categories.Queries;
using CSharpApp.Application.Products.Commands;
using CSharpApp.Application.Products.Queries;
using CSharpApp.Core.Dtos.Category;
using CSharpApp.Core.Dtos.Product;
using MediatR;
using FluentValidation;

namespace CSharpApp.Api
{
    public static class ApiEndpoints
    {
        public static WebApplication RouteApiEndpoints(this WebApplication app)
        {
            var versioned = app.NewVersionedApi();


            //getAllProducts
            versioned.MapGet("api/v{version:apiVersion}/getproducts", async (ISender mediator, CancellationToken ct) =>
            {
                var products = await mediator.Send(new GetAllProductsQuery(), ct);
                return Results.Ok(products);
            })
                .WithName("GetProducts")
                .HasApiVersion(1.0);


            //GetProductById
            versioned.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (int id, ISender mediator, CancellationToken ct) =>
            {
                var product = await mediator.Send(new GetProductByIdQuery(id), ct);
                if (product is null)
                    return Results.NotFound();
                return Results.Ok(product);
            })
                .WithName("GetProduct")
                .HasApiVersion(1.0);


            //CreateProduct
            versioned.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductRequest request, ISender mediator, IValidator<CreateProductRequest> validator, CancellationToken ct) =>
            {
                var result = await validator.ValidateAsync(request, ct);

                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var created = await mediator.Send(new CreateProductCommand(request), ct);
                return Results.Ok(created);
            })
                .WithName("CreateProduct")
                .HasApiVersion(1.0);



            //GetAllCategories
            versioned.MapGet("api/v{version:apiVersion}/getcategories", async (ISender mediator, CancellationToken ct) =>
            {
                var categories = await mediator.Send(new GetAllCategoriesQuery(), ct);
                return Results.Ok(categories);
            })
                .WithName("GetCategories")
                .HasApiVersion(1.0);



            //GetCategoryById
            versioned.MapGet("api/v{version:apiVersion}/getcategory/{id:int}", async (int id, ISender mediator, CancellationToken ct) =>
            {
                var category = await mediator.Send(new GetCategoryByIdQuery(id), ct);
                if (category is null)
                    return Results.NotFound();
                return Results.Ok(category);
            })
                .WithName("GetCategory")
                .HasApiVersion(1.0);



            //CreateCategory
            versioned.MapPost("api/v{version:apiVersion}/createcategory", async (CreateCategoryRequest request, ISender mediator, IValidator<CreateCategoryRequest> validator, CancellationToken ct) =>
            {
                var result = await validator.ValidateAsync(request, ct);

                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var created = await mediator.Send(new CreateCategoryCommand(request), ct);
                return Results.Ok(created);
            })
                .WithName("CreateCategory")
                .HasApiVersion(1.0);
            return app;
        }
    }
}
