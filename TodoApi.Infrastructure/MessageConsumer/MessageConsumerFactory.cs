using System;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Infrastructure.AWS.Options;
using TodoApi.Infrastructure.Enums;
using TodoApi.Infrastructure.Options;
using TodoApi.Infrastructure.Options.AWS;
using TodoApi.Infrastructure.Options.Azure;

namespace TodoApi.Infrastructure.MessageConsumer
{
    public class MessageConsumerFactory : IMessageConsumerFactory
	{
        private readonly ILogger<MessageConsumerFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        public MessageConsumerFactory(ILogger<MessageConsumerFactory> logger, IServiceProvider serviceProvider, IMediator mediator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        

        public IMessageConsumer CreateConsumer(AppSettings appSettings)
        {
            if (appSettings.EnvironmentType == EnvironmentType.Azure)
            {
                IOptions<AzureServiceBusOptions> azureServiceBustOptions = (IOptions<AzureServiceBusOptions>)_serviceProvider.GetService(typeof(IOptions<AzureServiceBusOptions>)) !;
                if(azureServiceBustOptions != null)
                    return new AzureMessageConsumer(_logger, azureServiceBustOptions);
            }
            var sqsOptions = (IOptions<SqsOptions>)_serviceProvider.GetService(typeof(IOptions<SqsOptions>))!;
            var credentialsOptions = (IOptions<AwsCredentialsOptions>)_serviceProvider.GetService(typeof(IOptions<AwsCredentialsOptions>))!;
            return new AwsMessageConsumer(_logger,_mediator, sqsOptions, credentialsOptions);
            
        }
    }
}

