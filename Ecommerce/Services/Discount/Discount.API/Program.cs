using Common.Logging;
using Common.Logging.Correlation;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

// Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

// Identity Server changes
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://identityserver:9011";
        options.Audience = "Discount";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true, // Indique que l'émetteur doit être validé
            ValidIssuer = "https://id-local.eshopping.com", // // L'émetteur exact attendu (issu de votre token)
            ValidateAudience = true, // Indique que l'audience doit être validée
            ValidAudience = "Discount", // Audience pour Basket.API
            ValidateLifetime = true, // Indique que la durée de vie doit être validée (exp, nbf)
            ValidateIssuerSigningKey = true // Indique que la clé de signature doit être validée
        };
    });

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);


// Register MediatR
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountCommandHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();
builder.Services.AddScoped<ICorrelationIDGenerator, CorrelationIDGenerator>();


var app = builder.Build();

//Migrate Database
app.MigrateDatabase<Program>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be through a gRPC Client");
    });
});

app.Run();
