using Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Application.Common.Behaviours
{
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
            catch (ValidationException ex)
            {
                var requestName = typeof(TRequest).Name;
                var failures = String.Join("", ex.Errors?.Select(t => "Per fushen-" + (t.Key == "" ? "pa_fushe" : t.Key) + " u thyen rregullat-" + String.Join("", t.Value?.ToArray())).ToArray());

                _logger.LogInformation("Credins.ePay Validation: Ndodhen thyerje te validimeve per {@name} me objekt: {@Request}. {@failures}",
                    typeof(TRequest).Name, request, failures);

                throw;
            }
            catch (Exception ex)
            {

                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Request: Unhandled Exception for Request {@Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
