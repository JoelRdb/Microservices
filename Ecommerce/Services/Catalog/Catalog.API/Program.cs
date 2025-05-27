using Asp.Versioning;
using Catalog.Application.Handlers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Catalog.API", Version = "v1" }); });

// Enregistrer Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Enregistrer  MediatR
var assemblies = new Assembly[]{
    Assembly.GetExecutingAssembly(),            // Catalog.API
    typeof(GetAllBrandsHandler).Assembly        // Catalog.Application
};                                              //Scanne tous ces projets pour trouver les handlers.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

// Enregistrer les services applicatif
builder.Services.AddScoped<IProductRepository, ProductRepositoy>();
builder.Services.AddScoped<IBrandRepository, ProductRepositoy>();
builder.Services.AddScoped<ITypeReposiroty, ProductRepositoy>();
builder.Services.AddScoped<ICatalogContext, CatalogContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
