using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Catalog.Application.Handlers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;
using System.Reflection;
using IApiVersionDescriptionProvider = Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Configuration;
using Common.Logging.Correlation;

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
})
// Add Versioned API Explorer to support versioning
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => 
{ 
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Catalog.API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] et votre token JWT dans le champs ci-dessous.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Ajoutez cette section pour indiquer que toutes les opérations (ou certaines) utilisent ce schéma de sécurité
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Fait référence au schéma défini ci-dessus
                }
            },
            new string[] { } // Scopes requis, vide pour l'instant si vous ne les gérez pas ici
        }
    });
});

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
builder.Services.AddScoped<ICorrelationIDGenerator, CorrelationIDGenerator>();

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
    options.Authority = "http://identityserver:9011";
    options.Audience = "Catalog";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer  = true, // Indique que l'émetteur doit être validé
            ValidIssuer = "https://id-local.eshopping.com", // L'émetteur exact attendu (issu de votre token)
            ValidateAudience = true, // Indique que l'audience doit être validée
            ValidAudience = "Catalog", // L'audience exacte attendue pour Ocelot
            ValidateLifetime = true, // Indique que la durée de vie doit être validée (exp, nbf)
            ValidateIssuerSigningKey = true // Indique que la clé de signature doit être validée
    };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "catalogapi.read"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
var nginxPath = "/catalog";
if (app.Environment.IsEnvironment("Local"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }); 
}

/// ---Configuration du pipeline Swagger UI ---
app.UseSwagger(); // Active le middleware Swagger
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
            $"Catalog API {description.GroupName.ToUpperInvariant()}");
    }
    options.RoutePrefix = string.Empty; // Permet d'accéder à index.html directement à la racine du chemin proxyfié (/catalog/)
    options.DocumentTitle = "Catalog API Documentation";

    // Configuration pour le bouton Authorize
    options.OAuthClientId("swagger_client"); // Si vous avez un client pour Swagger dans IdentityServer
    options.OAuthAppName("Swagger UI for Catalog.API");
    options.OAuthUsePkce();
});
// --- Fin Configuration du pipeline Swagger UI ---


app.UseRouting(); // Important : doit être avant UseAuthentication/UseAuthorization
app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapControllers();

app.Run();