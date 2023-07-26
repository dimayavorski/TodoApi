using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options.Azure;

namespace TodoApi.Infrastructure.MessageConsumer
{
    public class AzureMessageConsumerService : IMessageConsumerService
	{
		public AzureMessageConsumerService(ILogger<MessageConsumerFactory> logger, IMediator mediator, IOptions<AzureServiceBusOptions> options)
		{
		}

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task ConsumeMessage(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}

