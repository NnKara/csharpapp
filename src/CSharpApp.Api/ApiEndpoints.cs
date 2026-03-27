using CSharpApp.Application.Helpers;
using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Category;
using CSharpApp.Core.Dtos.Product;

namespace CSharpApp.Api
{
    public static class ApiEndpoints
    {
        public static WebApplication RouteApiEndpoints(this WebApplication app)
        {
            var versioned = app.NewVersionedApi();


            versioned.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
            {
                var products = await productsService.GetAllAsync();
                return Results.Ok(products);
            })
                .WithName("GetProducts")
                .HasApiVersion(1.0);


            versioned.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (int id, IProductsService productsService,
                    CancellationToken cancellationToken) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { message = "ID must be greater than 0" });
                var product = await productsService.GetByIdAsync(id, cancellationToken);
                if (product is null)
                    return Results.NotFound();
                return Results.Ok(product);
            })
                .WithName("GetProduct")
                .HasApiVersion(1.0);


            versioned.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductRequest request,
                    IProductsService productsService,
                    CancellationToken cancellationToken) =>
            {
                var errors = CreateProductRequestValidator.ValidateCreateProdReq(request);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });
                var created = await productsService.CreateAsync(request, cancellationToken);
                return Results.Ok(created);
            })
                .WithName("CreateProduct")
                .HasApiVersion(1.0);


            versioned.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
            {
                var categories = await categoriesService.GetAllAsync();
                return Results.Ok(categories);
            })
                .WithName("GetCategories")
                .HasApiVersion(1.0);


            versioned.MapGet("api/v{version:apiVersion}/getcategory/{id:int}", async (int id, ICategoriesService categoriesService,
                    CancellationToken cancellationToken) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { message = "ID must be greater than 0" });
                var category = await categoriesService.GetByIdAsync(id, cancellationToken);
                if (category is null)
                    return Results.NotFound();
                return Results.Ok(category);
            })
                .WithName("GetCategory")
                .HasApiVersion(1.0);


            versioned.MapPost("api/v{version:apiVersion}/createcategory", async (CreateCategoryRequest request,
                    ICategoriesService categoriesService,
                    CancellationToken ct) =>
            {
                var errors = CreateCategoryRequestValidator.ValidateCreateCategoryReq(request);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });
                var created = await categoriesService.CreateAsync(request, ct);
                return Results.Ok(created);
            })
                .WithName("CreateCategory")
                .HasApiVersion(1.0);
            return app;
        }
    }
}
