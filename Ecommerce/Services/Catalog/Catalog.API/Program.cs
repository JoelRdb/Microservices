using Asp.Versioning;
using Catalog.Application.Handlers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

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

// Implementing Authorize Filter for use Identity Server
var userPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
builder.Services.AddControllers(config =>
    {
        config.Filters.Add(new AuthorizeFilter(userPolicy));
    });

// Identity Server changes : after install of Ecommerce.Identity with Duende
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:9009";
        options.Audience = "Catalog";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Active Swagger tout le temps
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

// Rediriger automatiquement "/" vers Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


app.UseAuthorization();

app.MapControllers();

app.Run();
