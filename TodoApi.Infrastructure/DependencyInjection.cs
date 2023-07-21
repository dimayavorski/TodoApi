using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Application.Common.Options.Aws;
using TodoApi.Application.Common.Options.AWS;
using TodoApi.Infrastructure.Factories;
using TodoApi.Infrastructure.Repositories;
using TodoApi.Infrastructure.Repositories.Aws;
using TodoApi.Infrastructure.Secrets;
using TodoApi.Infrastructure.Services;
using TodoApi.Infrastructure.Services.Aws;
using TodoApi.Infrastructure.Services.Azure;

namespace TodoApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, AppSettings appSettings,
            IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IFileServiceFactory, FileServiceFactory>();
            serviceCollection.AddSingleton<IRepositoryFactory, RepositoryFactory>();
            serviceCollection.AddSingleton<IMessagePublisherFactory, MessagePublisherServiceFactory>();
            if (appSettings.EnvironmentType.ToString().StartsWith(EnvironmentType.AWS.ToString()))
            {
                var credentialsService = serviceCollection.BuildServiceProvider().GetRequiredService<ICredentialsService>();
                var credentials = credentialsService.GetCredentials();
                

                serviceCollection.AddSingleton<IAmazonS3, AmazonS3Client>();
                serviceCollection.AddS3Service(credentials);
                serviceCollection.AddDynamoDbServices(credentials);
                serviceCollection.AddSnsServices(credentials, configuration);


            }
            else
            {
                serviceCollection.AddAzureBlobStorageService();
            }
            return serviceCollection;
        }

        public static IServiceCollection AddS3Service(this IServiceCollection serviceCollection, AWSCredentials credentials)
        {

            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };
            var awsS3Client = new AmazonS3Client(credentials, config);
            serviceCollection.AddSingleton<IAmazonS3>(awsS3Client);
            serviceCollection.AddSingleton<S3FileService>();

            return serviceCollection;
        }

        public static IServiceCollection AddSnsServices(this IServiceCollection serviceCollection, AWSCredentials credentials,
            IConfiguration configuration)
        {

            var config = new AmazonSimpleNotificationServiceConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };
            var awsS3Client = new AmazonSimpleNotificationServiceClient(credentials, config);
            serviceCollection.AddSingleton<IAmazonSimpleNotificationService>(awsS3Client);
            serviceCollection.AddSingleton<AwsMessagePublisherService>();
            serviceCollection.Configure<AwsSnsOptions>(configuration.GetSection(nameof(AwsSnsOptions)));
            

            return serviceCollection;
        }

        public static IServiceCollection AddAzureBlobStorageService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<AzureBlobStorageFileService>();
            return serviceCollection;
        }

        public static IServiceCollection AddDynamoDbServices(this IServiceCollection serviceCollection, AWSCredentials credentials)
        {
            serviceCollection.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var config = new AmazonDynamoDBConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.EUWest2
                };
                return new AmazonDynamoDBClient(credentials, config);
            });
            serviceCollection.AddSingleton<TodoRepositoryDynamoDb>();

            return serviceCollection;
        }
    }
}

