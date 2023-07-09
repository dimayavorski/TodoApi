using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Infrastructure.AWS.Options;
using TodoApi.Infrastructure.Options.Azure;

namespace TodoApi.Infrastructure.MessageConsumer
{
    public class AzureMessageConsumer : IMessageConsumer
	{
		public AzureMessageConsumer(ILogger<MessageConsumerFactory> logger, IOptions<AzureServiceBusOptions> options)
		{
		}

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        Task IMessageConsumer.ConsumeMessage(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

