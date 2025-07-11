using Common.Logging.Correlation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyMethod().AllowAnyOrigin();
    });
});

var authScheme = "ECommerceGatewayAuthScheme";
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(authScheme, options =>
//    {
//        options.Authority = "http://identityserver:9011"; //permet à Ocelot de se connecter à identity server
//        options.Audience = "ECommerceGateway";
//        options.RequireHttpsMetadata = false;
//        // ***  ***
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuer = true, // Indique que l'émetteur doit être validé
//            ValidIssuer = "https://id-local.eshopping.com", // L'émetteur exact attendu (issu de votre token)
//            ValidateAudience = true, // Indique que l'audience doit être validée
//            ValidAudience = "ECommerceGateway", // L'audience exacte attendue pour Ocelot
//            ValidateLifetime = true, // Indique que la durée de vie doit être validée (exp, nbf)
//            ValidateIssuerSigningKey = true // Indique que la clé de signature doit être validée
//        };
//    });


builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(o => o.WithDictionaryHandle());

builder.Services.AddScoped<ICorrelationIDGenerator, CorrelationIDGenerator>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddCorrelationIdMiddleware();
app.UseRouting();
app.UseCors("CorsPolicy");
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        await context.Response.WriteAsync("Hello Ocelot");
    }
    else
    {
        await next();
    }
});

await app.UseOcelot();



app.Run();