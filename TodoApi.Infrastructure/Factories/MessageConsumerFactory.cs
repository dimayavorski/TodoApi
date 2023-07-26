using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Application.Common.Options.Aws;
using TodoApi.Application.Common.Options.Azure;
using TodoApi.Infrastructure.Services.Aws;
using TodoApi.Infrastructure.Services.Azure;

namespace TodoApi.Infrastructure.MessageConsumer
{
    public class MessageConsumerFactory : IMessageConsumerFactory
	{
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _appSettings;
        public MessageConsumerFactory(IServiceProvider serviceProvider, AppSettings appSettings)
        {
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
  
        }

        public IMessageConsumerService CreateConsumer(AppSettings appSettings)
        {
            return _appSettings.EnvironmentType switch
            {
                EnvironmentType.AWS | EnvironmentType.AWSLocal
                    => (IMessageConsumerService)_serviceProvider.GetRequiredService(typeof(AwsMessageConsumerService)),
                EnvironmentType.Azure | EnvironmentType.AzureLocal
                   => (IMessageConsumerService)_serviceProvider.GetRequiredService(typeof(AzureMessageConsumerService)),
                _ => throw new NotImplementedException("There is no consumer service implementation for this configuration")

            };
        }
    }
}

