using System;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoApi.Infrastructure.Messages;

namespace TodoApi.Infrastructure.Handlers
{
    public class ProfileCreatedHandler : IRequestHandler<ProfileCreated>
	{
        private readonly ILogger<ProfileCreatedHandler> _logger;
        public ProfileCreatedHandler(ILogger<ProfileCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProfileCreated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request Handlerd");
            return Task.CompletedTask;
        }
    }
}

