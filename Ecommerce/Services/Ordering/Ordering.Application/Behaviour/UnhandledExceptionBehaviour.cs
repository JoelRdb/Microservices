using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Handlers;
using System.Net;
using System.Text;

namespace Ordering.Application.Behaviour
{
    //Si un handler plante ou si une exception inattendue est levée n’importe où dans la requête,
    //ce comportement la capture et la journalise.
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, $"Unhandled exception occured with Request Name: {requestName}, {request}");
                throw;
            }
        }
    }
}
