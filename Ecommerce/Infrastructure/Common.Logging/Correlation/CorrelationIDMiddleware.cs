using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Diagnostics;
using System.Xml;

namespace Common.Logging.Correlation
{ // Définir un middleware qui gère l'ID de correlation (X-Correlation-Id) pour chaque requete HTTP
  // pour suivre une requete à travers plusieurs services (tracing distribué).
    public class CorrelationIDGeneratorMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _correlationIdHeader = "X-Correlation-Id";

        public CorrelationIDGeneratorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICorrelationIDGenerator correlationIDGenerator)
        {
            var correlationId = GetCorrelationId(context, correlationIDGenerator);
            AddCorrelationIdHeader(context, correlationId);
            await _next(context);
        }

        private static StringValues GetCorrelationId(HttpContext context, ICorrelationIDGenerator correlationIDGenerator)
        {
            //Vérifie si un en-tete "X-Correlation-Id" est présent dans la requete entrante
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            { // si oui, il conserve
                correlationIDGenerator.Set(correlationId);
                return correlationId;
            }
            else
            { // Si non, il en génère un nouveau via ICorrelationIDGenerator
                return correlationIDGenerator.Get();
            }
        }

        //Ajouter l'ID dans les en-tetes de réponse HTTP juste avant l'envoi au client
        private static void AddCorrelationIdHeader(HttpContext context, StringValues correlationId)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(_correlationIdHeader, new[] { correlationId.ToString() });
                return Task.CompletedTask;
            });
            //Concrètement, pourquoi c’est utile ?
            //Pour corréler tous les logs générés par une requête HTTP via un ID unique.
            //Pour faciliter le debug dans les environnements distribués (microservices).
            //Pour permettre à un service A de passer l'ID à un service B, etc.
        }
    }
}
