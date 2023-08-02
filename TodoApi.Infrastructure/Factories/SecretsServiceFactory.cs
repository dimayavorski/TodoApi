using System;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Infrastructure.Services.Aws;
using TodoApi.Infrastructure.Services.Azure;

namespace TodoApi.Infrastructure.Factories
{
    public class SecretsServiceFactory : ISecretsServiceFactory
    {
        private readonly AppSettings _appSettings;
        private readonly IServiceProvider _serviceProvider;
        public SecretsServiceFactory(AppSettings appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings;
            _serviceProvider = serviceProvider;
        }

        public ISecretsService GetSecretsService()
        {
            return _appSettings.EnvironmentType switch
            {
                EnvironmentType.AWS or EnvironmentType.AWSLocal
                    => (ISecretsService)_serviceProvider.GetRequiredService(typeof(AwsSecretsService)),
                EnvironmentType.Azure or EnvironmentType.AzureLocal
                   => (ISecretsService)_serviceProvider.GetRequiredService(typeof(AzureSecretsService)),
                _ => throw new NotImplementedException("There is no secrets service implementation for this configuration")

            };
        }
    }
}

