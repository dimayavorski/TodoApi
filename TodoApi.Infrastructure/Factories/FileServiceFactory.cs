using Microsoft.Extensions.DependencyInjection;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Infrastructure.Services.Aws;
using TodoApi.Infrastructure.Services.Azure;

namespace TodoApi.Infrastructure.Services
{
    public class FileServiceFactory : IFileServiceFactory
	{
        private readonly AppSettings _appSettings;
        private readonly IServiceProvider _serviceProvider;
		public FileServiceFactory(AppSettings appSettings, IServiceProvider serviceProvider)
		{
            _appSettings = appSettings;
            _serviceProvider = serviceProvider;
		}

        public IFileService GetFileService()
        {
            return _appSettings.EnvironmentType switch
            {
                EnvironmentType.AWS or EnvironmentType.AWSLocal
                    => (IFileService)_serviceProvider.GetRequiredService(typeof(S3FileService)),
                EnvironmentType.Azure or EnvironmentType.AzureLocal
                   => (IFileService)_serviceProvider.GetRequiredService(typeof(AzureBlobStorageFileService)),
                _ => throw new NotImplementedException("There is no file service implementation for this configuration")

            };
        }
    }
}

