using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        // Déclare une fonction réutilisable qui configure Serilog.
        /* HostBuilderContext → fournit l’environnement (Development, Production, etc.)
           LoggerConfiguration → l’objet à configurer
        */
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfiguration) =>
            {
                var env = context.HostingEnvironment;
                loggerConfiguration.MinimumLevel.Information() //Le niveau de log minimum global est Information (ignore Debug et Trace par défaut).
                .Enrich.FromLogContext() //Ajoute automatiquement des propriétés du contexte de log, comme RequestId, UserId, etc., quand elles sont disponibles.
                .Enrich.WithProperty("ApplicationName", env.ApplicationName) // Ajoute un propriété personnalisé à chaque log :le nom de l'application
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName) //// Ajoute un propriété personnalisé à chaque log :le nom de l'application
                .Enrich.WithExceptionDetails()  //Utilise le package Serilog.Exceptions pour enrichir les logs d’erreur avec tous les détails des exceptions.
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)   // Les logs ASP.NET Core système ne seront logués qu’à partir de Warning pour éviter de polluer la console avec du bruit.
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console(); //Envoie les logs formatés dans la console (très utile en développement et dans Docker).
                /*
                    Quand l’app tourne en environnement Development :
                    Les microservices Catalog, Basket, Discount, Ordering ont un niveau minimum Debug
                    Pratique pour avoir plus de détails pendant le dev, sans changer le niveau global.
                */
                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfiguration.MinimumLevel.Override("Catalog", Serilog.Events.LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Basket", Serilog.Events.LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Discount", Serilog.Events.LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Ordering", Serilog.Events.LogEventLevel.Debug);
                }

                // Elastic search
                var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
                if(!string.IsNullOrEmpty(elasticUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUrl))
                        {
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                            IndexFormat = "ecommerce-Logs-{0:yyy.MM.dd}",
                            MinimumLogEventLevel = LogEventLevel.Debug
                        });      
                }
            };
    }
}
