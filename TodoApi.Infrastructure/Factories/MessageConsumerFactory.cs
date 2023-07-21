using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Application.Common.Options.Aws;
using TodoApi.Application.Common.Options.Azure;
using TodoApi.Infrastructure.Services.Aws;

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

        

        public IMessageConsumerService CreateConsumer(AppSettings appSettings)
        {
            if (appSettings.EnvironmentType == EnvironmentType.Azure)
            {
                IOptions<AzureServiceBusOptions> azureServiceBustOptions = (IOptions<AzureServiceBusOptions>)_serviceProvider.GetService(typeof(IOptions<AzureServiceBusOptions>)) !;
                if(azureServiceBustOptions != null)
                    return new AzureMessageConsumerService(_logger, azureServiceBustOptions);
            }
            var sqsOptions = (IOptions<SqsOptions>)_serviceProvider.GetService(typeof(IOptions<SqsOptions>))!;
            var credentialsOptions = (IOptions<AwsCredentialsOptions>)_serviceProvider.GetService(typeof(IOptions<AwsCredentialsOptions>))!;
            return new AwsMessageConsumerService(_logger,_mediator, sqsOptions, credentialsOptions);
            
        }
    }
}

