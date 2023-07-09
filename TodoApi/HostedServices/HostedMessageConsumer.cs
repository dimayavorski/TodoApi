using System;
using TodoApi.Infrastructure.MessageConsumer;
using TodoApi.Infrastructure.Options;

namespace TodoApi.HostedServices
{
    public class HostedMessageConsumer : BackgroundService
	{
        private readonly IMessageConsumerFactory _messageConsumerFactory;
        private readonly AppSettings _appSettings;
		public HostedMessageConsumer(IMessageConsumerFactory messageConsumerFactory, AppSettings appSettings)
		{
            _messageConsumerFactory = messageConsumerFactory;
            _appSettings = appSettings;
		}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = _messageConsumerFactory.CreateConsumer(_appSettings);
            await consumer.Connect();
            while(!stoppingToken.IsCancellationRequested) {


                await consumer.ConsumeMessage(stoppingToken);


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

