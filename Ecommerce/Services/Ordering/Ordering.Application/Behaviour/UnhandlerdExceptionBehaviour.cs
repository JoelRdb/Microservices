using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Handlers;

namespace Ordering.Application.Behaviour
{
    public class UnhandlerdExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandlerdExceptionBehaviour(ILogger<TRequest> logger)
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
