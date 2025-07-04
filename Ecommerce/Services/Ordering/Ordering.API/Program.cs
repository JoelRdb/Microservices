using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Common.Logging;
using EventBus.Messages.E_vents.Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);


builder.Services.AddControllers();
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

//Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

// Identity Server
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://id-local.eshopping.com:44344";
        options.Audience = "Ordering";
    });
// Application Services
builder.Services.AddApplicationServices();

// Infra services
builder.Services.AddInfraServices(builder.Configuration);

//Consumer class
builder.Services.AddScoped<BasketOrderingConsumer>();
builder.Services.AddScoped<BasketOrderingConsumerV2>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ordering.API", Version = "v1" }); });

//Mass Transit
builder.Services.AddMassTransit(config =>
{
    // Mark this as consumer
    config.AddConsumer<BasketOrderingConsumer>();
    config.AddConsumer<BasketOrderingConsumerV2>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        // provide the queue name with consumer settings
        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });
        //V2 Version 
        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueueV2, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumerV2>(ctx);
        });
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

//Apply Db Migration
app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
var nginxPath = "/ordering";
if (app.Environment.IsEnvironment("Local"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
                $"Ordering API {description.GroupName.ToUpperInvariant()}");
            options.RoutePrefix = string.Empty;
        }

        options.DocumentTitle = "Catalog API Documentation";
    });
}
// Rediriger automatiquement "/" vers Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;

});
app.UseAuthorization();

app.MapControllers();

app.Run();
