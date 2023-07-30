using Microsoft.Extensions.DependencyInjection;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Infrastructure.Repositories.Aws;
using TodoApi.Infrastructure.Repositories.Azure;

namespace TodoApi.Infrastructure.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly AppSettings _appSettings;
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(AppSettings appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings;
            _serviceProvider = serviceProvider;
        }

        public IRepository GetRepository()
        {
            return _appSettings.EnvironmentType switch
            {
                EnvironmentType.AWS | EnvironmentType.AWSLocal
                    => (IRepository)_serviceProvider.GetRequiredService(typeof(TodoRepositoryDynamoDb)),
                EnvironmentType.Azure | EnvironmentType.AzureLocal
                   => (IRepository)_serviceProvider.GetRequiredService(typeof(TodoRepositoryCosmosDb)),
                _ => throw new NotImplementedException("There is no repository   implementation for this configuration")

            };
        }
    }
}

