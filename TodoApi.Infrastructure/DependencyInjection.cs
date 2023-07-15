using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Infrastructure.Enums;
using TodoApi.Infrastructure.Options;
using TodoApi.Infrastructure.Secrets;
using TodoApi.Infrastructure.Services;
using TodoApi.Infrastructure.Services.Aws;
using TodoApi.Infrastructure.Services.Azure;

namespace TodoApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, AppSettings appSettings)
        {
            serviceCollection.AddSingleton<IFileServiceFactory, FileServiceFactory>();
            if (appSettings.EnvironmentType.ToString().StartsWith(EnvironmentType.AWS.ToString()))
            {

                serviceCollection.AddSingleton<IAmazonS3, AmazonS3Client>();
                serviceCollection.AddS3Service();
                

            }
            else
            {
                serviceCollection.AddAzureBlobStorageService();
            }
            return serviceCollection;
        }

        public static IServiceCollection AddS3Service(this IServiceCollection serviceCollection)
        {
            var credentialsService = serviceCollection.BuildServiceProvider().GetRequiredService<ICredentialsService>();
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };
            var awsS3Client = new AmazonS3Client(credentialsService.GetCredentials(), config);
            serviceCollection.AddSingleton<IAmazonS3>(awsS3Client);
            serviceCollection.AddSingleton<S3FileService>();

            return serviceCollection;
        }

        public static IServiceCollection AddAzureBlobStorageService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<AzureBlobStorageFileService>();
            return serviceCollection;
        }
    }
}

