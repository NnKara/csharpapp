using CSharpApp.Api.Configuration;
using CSharpApp.Api.PerformanceMiddleware;
using CSharpApp.Application.Helpers;
using CSharpApp.Application.Interfaces.Categories;
using CSharpApp.Application.Interfaces.Products;
using CSharpApp.Core.Dtos.Product;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration(builder.Configuration);
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddSingleton<RequestPerformanceMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<RequestPerformanceMiddleware>();
app.UseGlobalExceptionHandling();


//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
    {
        var products = await productsService.GetAllAsync();
        return Results.Ok(products);
    })
    .WithName("GetProducts")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (int id,IProductsService productsService,
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

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductRequest request,
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

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.GetAllAsync();
    return Results.Ok(categories);
})
.WithName("GetCategories")
.HasApiVersion(1.0);

app.Run();