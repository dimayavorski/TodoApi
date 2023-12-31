﻿using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SecretsManager;
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
using TodoApi.Infrastructure.ConfigurationProviders;
using TodoApi.Infrastructure.MessageConsumer;
using Amazon.SQS;
using Azure.Identity;
using TodoApi.Application.Common.Options.Azure;
using TodoApi.Infrastructure.Repositories.Azure;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;

namespace TodoApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, AppSettings appSettings,
            IConfigurationBuilder configurationBuilder)
        {
            serviceCollection.AddSingleton<IFileServiceFactory, FileServiceFactory>();
            serviceCollection.AddSingleton<IRepositoryFactory, RepositoryFactory>();
            serviceCollection.AddSingleton<IMessagePublisherFactory, MessagePublisherServiceFactory>();
            serviceCollection.AddSingleton<IMessageConsumerFactory, MessageConsumerFactory>();
            serviceCollection.AddSingleton<ICredentialsService, CredentialsService>();
            var configuration = configurationBuilder.Build();
            if (appSettings.EnvironmentType.ToString().StartsWith(EnvironmentType.AWS.ToString()))
            {
                serviceCollection.AddSingleton<IAmazonS3, AmazonS3Client>();
                serviceCollection.AddS3Service();
                serviceCollection.AddDynamoDbServices();
                serviceCollection.AddSnsServices(configuration);
            }
            else
            {
                serviceCollection.AddAzureBlobStorageService();
                serviceCollection.AddCosmosDbServices(configuration);
                serviceCollection.AddAzureServiceBusServices(configuration);
                serviceCollection.AddAzureBlobStorageServices(configuration);
            }
            return serviceCollection;
        }

        public static IServiceCollection AddS3Service(this IServiceCollection serviceCollection)
        {

            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2,
            };
            var awsS3Client = new AmazonS3Client(config);
            serviceCollection.AddSingleton<IAmazonS3>(awsS3Client);
            serviceCollection.AddSingleton<S3FileService>();

            return serviceCollection;
        }

        public static IServiceCollection AddSnsServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            var config = new AmazonSimpleNotificationServiceConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };
            var awsS3Client = new AmazonSimpleNotificationServiceClient(config);
            serviceCollection.AddSingleton<IAmazonSimpleNotificationService>(awsS3Client);
            serviceCollection.AddSingleton<AwsMessagePublisherService>();
            serviceCollection.Configure<AwsSnsOptions>(configuration.GetSection(nameof(AwsSnsOptions)));


            return serviceCollection;
        }

        public static IServiceCollection AddSQSServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            var config = new AmazonSQSConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };
            var awsS3Client = new AmazonSQSClient(config);
            serviceCollection.AddSingleton<IAmazonSQS>(awsS3Client);
            serviceCollection.AddSingleton<AwsMessageConsumerService>();
            serviceCollection.Configure<SqsOptions>(configuration.GetSection(nameof(SqsOptions)));


            return serviceCollection;
        }

        public static IServiceCollection AddAzureBlobStorageService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<AzureBlobStorageFileService>();
            return serviceCollection;
        }

        public static IServiceCollection AddDynamoDbServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var config = new AmazonDynamoDBConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.EUWest2
                };
                return new AmazonDynamoDBClient(config);
            });
            serviceCollection.AddSingleton<TodoRepositoryDynamoDb>();

            return serviceCollection;
        }

        public static IServiceCollection AddCosmosDbServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var cosmosDbOptions = new AzureCosmosDbOptions();
            configuration.GetSection(AzureCosmosDbOptions.SectionName).Bind(cosmosDbOptions);

            if (string.IsNullOrEmpty(cosmosDbOptions.ConnectionString) || string.IsNullOrEmpty(cosmosDbOptions.ContainerName) || string.IsNullOrEmpty(cosmosDbOptions.DatabaseName))
                throw new ApplicationException("Error while configuring Cosmos Db");

            serviceCollection.Configure<AzureCosmosDbOptions>(options =>
            {
                options.ContainerName = cosmosDbOptions.ContainerName;
                options.ConnectionString = cosmosDbOptions.ConnectionString;
                options.DatabaseName = cosmosDbOptions.DatabaseName;
            });
            serviceCollection.AddDbContext<ApplicationContenxt>();


            serviceCollection.AddTransient<TodoRepositoryCosmosDb>();

            return serviceCollection;
        }


        public static IConfigurationBuilder AddKeyVaultServices(this IConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            string keyVaultUri = configuration.GetValue<string>("KeyVaultUri")!;
            builder.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());

            return builder;
        }

        public static IConfigurationBuilder AddAwsSecretsServices(this IConfigurationBuilder builder)
        {
            var configurationSource = new AmazonConfigurationSource();
            builder.Add(configurationSource);

            return builder;
        }

        public static IServiceCollection AddAzureServiceBusServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<AzureServiceBusOptions>(configuration.GetSection(AzureServiceBusOptions.SectionName));
            serviceCollection.AddSingleton<AzureMessagePublisherService>();
            serviceCollection.AddSingleton<AzureMessageConsumerService>();

            return serviceCollection;
        }

        public static IServiceCollection AddAzureBlobStorageServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var azureBlobStorageOptions = new AzureBlobStorageOptions();
            configuration.GetSection(AzureBlobStorageOptions.SectionName).Bind(azureBlobStorageOptions);
            serviceCollection.Configure<AzureBlobStorageOptions>(options =>
            {
                options.ServiceUrl = azureBlobStorageOptions.ServiceUrl;
                options.ContainerName = azureBlobStorageOptions.ContainerName;
            });

            var blobServiceClient = new BlobServiceClient(new Uri(azureBlobStorageOptions.ServiceUrl), new DefaultAzureCredential());
            serviceCollection.AddSingleton(blobServiceClient);

            return serviceCollection;
        }
    }
}

