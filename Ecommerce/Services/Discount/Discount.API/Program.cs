using Common.Logging;
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
        options.Authority = "https://id-local.eshopping.com:44344";
        options.Audience = "Discount";
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
