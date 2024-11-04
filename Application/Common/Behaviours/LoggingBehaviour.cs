using Application.Common.Interfaces;
using Application.Entities.Products.Commands;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger;
        private readonly ICurrentActorService _currentUserService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentActorService currentUserService
            )
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var actor = (_currentUserService.IsClient ? _currentUserService.ClientId : _currentUserService.UserId) ?? string.Empty;
            string userName = string.Empty;

            if (!string.IsNullOrEmpty(actor))
            {
                userName = (_currentUserService.IsClient ? _currentUserService.ClientId : _currentUserService.UserId) ?? string.Empty;
            }



            _logger.LogInformation("Request: {@Name} {@UserId} {@UserName} {@Request}",
            requestName, actor, userName, request);

        }
    }
}
