using System;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Infrastructure.Services.Aws;
using TodoApi.Infrastructure.Services.Azure;

namespace TodoApi.Infrastructure.Factories
{
    public class MessagePublisherServiceFactory : IMessagePublisherFactory
	{
        private readonly AppSettings _appSettings;
        private readonly IServiceProvider _serviceProvider;
        public MessagePublisherServiceFactory(AppSettings appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings;
            _serviceProvider = serviceProvider;
        }

        public IMessagePublisherService GetMessagePublisher()
        {
            return _appSettings.EnvironmentType switch
            {
                EnvironmentType.AWS or EnvironmentType.AWSLocal
                    => (IMessagePublisherService)_serviceProvider.GetRequiredService(typeof(AwsMessagePublisherService)),
                EnvironmentType.Azure or EnvironmentType.AzureLocal
                   => (IMessagePublisherService)_serviceProvider.GetRequiredService(typeof(AzureMessagePublisherService)),
                _ => throw new NotImplementedException("There is no file service implementation for this configuration")

            };
        }
    }
}

