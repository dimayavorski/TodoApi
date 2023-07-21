using System;
using TodoApi.Application.Common.Interfaces;

namespace TodoApi.Infrastructure.Services.Azure
{
    public class AzureMessagePublisherService : IMessagePublisherService
	{
		public AzureMessagePublisherService()
		{
		}

        public Task PublishMessage<T>(T message) where T : IQueueMessage
        {
            throw new NotImplementedException();
        }
    }
}

