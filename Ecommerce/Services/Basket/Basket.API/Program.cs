using Basket.Application.GrpcService;
using Basket.Application.Hadlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Proto;
using MassTransit;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using IApiVersionDescriptionProvider = Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);


// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
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

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket.API", Version = "v1" });
    c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket.API", Version = "v2" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] et votre token JWT dans le champs ci-dessous.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    //Ajouter cette section pour indiquer que toutes les op�rations (ou certains) utilisent ce sch�ma de s�curit�
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer" //Fait r�f�rence au sch�ma ci-dessus
            }
        },
            new string[] {} //Scopes requis, vide pour l'instant si vous ne les g�rez pas ici
        }
    });

    //Include XML comments 
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    //Configure Swagger to use versioning
    c.DocInclusionPredicate((version, ApiDescription) =>
    {
        if (!ApiDescription.TryGetMethodInfo(out var methodInfo))
        {
            return false;
        }
        var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(true)
                        .OfType<Asp.Versioning.ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);
        return versions?.Any(v => $"v{v.ToString()}" == version) ?? false;
    });
});


// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);


// Register MediatR
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateShoppingCartHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

// Register Redis : Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

// Application Services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    cfg => cfg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

// Implementing Authorize Filter for use Identity Server
var userPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new AuthorizeFilter(userPolicy));
});

// Identity Server : after install project of Ecommerce.Identity with Duende
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://identityserver:9011";
        options.Audience = "Basket";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true, // Indique que l'�metteur doit �tre valid�
            ValidIssuer = "https://id-local.eshopping.com", // // L'�metteur exact attendu (issu de votre token)
            ValidateAudience = true, // Indique que l'audience doit �tre valid�e
            ValidAudience = "Basket", // Audience pour Basket.API
            ValidateLifetime = true, // Indique que la dur�e de vie doit �tre valid�e (exp, nbf)
            ValidateIssuerSigningKey = true // Indique que la cl� de signature doit �tre valid�e
        };
    });


builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ct, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
var nginxPath = "/basket";
if (app.Environment.IsEnvironment("Local"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
    });
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
                $"Basket API {description.GroupName.ToUpperInvariant()}");
            options.RoutePrefix = string.Empty;
        }

        options.DocumentTitle = "Basket API Documentation";

        //Configuration pour le bouton Authorize
        options.OAuthClientId("swagger_client");
        options.OAuthAppName("Swagger UI for Basket.API");
        options.OAuthUsePkce();
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
