using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options.Azure;

namespace TodoApi.Infrastructure.Services.Azure
{
    public class AzureMessagePublisherService : IMessagePublisherService
	{
        private readonly ServiceBusClient _serviceBusClient;
        private readonly AzureServiceBusOptions _serviceBusOptions;
        private readonly ServiceBusSender _serviceBusSender;
		public AzureMessagePublisherService(IOptions<AzureServiceBusOptions> serviceBusOptions)
		{
            _serviceBusOptions = serviceBusOptions.Value ?? throw new ApplicationException($"{nameof(AzureServiceBusOptions)} config is null");
            _serviceBusClient = new ServiceBusClient(_serviceBusOptions.ConnectionString);
            _serviceBusSender = _serviceBusClient.CreateSender(_serviceBusOptions.TopicName);
		}

        public async Task PublishMessage<T>(T message) where T : IQueueMessage
        {
            string messageAsJson = JsonConvert.SerializeObject(message);
            
            var serviceBusMessage = new ServiceBusMessage(messageAsJson);

            serviceBusMessage.ApplicationProperties.Add("MessageType", typeof(T).Name);
            await _serviceBusSender.SendMessageAsync(serviceBusMessage);
        }
    }
}

