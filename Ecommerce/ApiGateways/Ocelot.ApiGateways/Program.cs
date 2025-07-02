using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

var authScheme = "ECommerceGatewayAuthScheme";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(authScheme, options =>
    {
        options.Authority = "https://localhost:9009";
        options.Audience = "ECommerceGateway";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://localhost:9009",
            ValidateAudience = true,
            ValidAudience = "ECommerceGateway",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });



builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(o => o.WithDictionaryHandle());



var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGet("/", () => "Hello Ocelot");

await app.UseOcelot();


app.Run();