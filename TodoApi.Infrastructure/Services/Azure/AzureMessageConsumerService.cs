using System.Text;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Messages;
using TodoApi.Application.Common.Options.Azure;

namespace TodoApi.Infrastructure.MessageConsumer
{
    public class AzureMessageConsumerService : IMessageConsumerService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly AzureServiceBusOptions _serviceBusOptions;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        public AzureMessageConsumerService(IOptions<AzureServiceBusOptions> serviceBusOptions)
        {
            _serviceBusOptions = serviceBusOptions.Value ?? throw new ApplicationException($"{nameof(AzureServiceBusOptions)} config is null");
            _serviceBusClient = new ServiceBusClient(_serviceBusOptions.ConnectionString);
            _serviceBusReceiver = _serviceBusClient.CreateReceiver(_serviceBusOptions.SubscriberName);
        }

        public async Task ConsumeMessage(CancellationToken cancellationToken)
        {
            var message = await _serviceBusReceiver.ReceiveMessageAsync();
            if (message != null)
            {
                var body = JsonConvert.DeserializeObject<TodoCreatedMessage>(Encoding.UTF8.GetString(message.Body));

            }
            await _serviceBusReceiver.CompleteMessageAsync(message);
        }


    }
}

