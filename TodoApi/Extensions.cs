using System;
using System.Net;
using Amazon.Runtime;
using Microsoft.AspNetCore.Diagnostics;
using TodoApi.Infrastructure.AWS.Options;
using TodoApi.Infrastructure.Enums;
using TodoApi.Infrastructure.MessageConsumer;
using TodoApi.Infrastructure.Options;
using TodoApi.Infrastructure.Options.AWS;
using TodoApi.Infrastructure.Secrets;

namespace TodoApi
{
    public static class Extensions
    {
        public static IServiceCollection AddCloudTypeBasedSettings(this IServiceCollection serviceCollection, IConfiguration configuration, AppSettings appSettings)
        {
            serviceCollection.AddTransient<ICredentialsService, CredentialsService>();
            if (appSettings.EnvironmentType.ToString().StartsWith(EnvironmentType.AWS.ToString()))
            {
                if (appSettings.EnvironmentType == EnvironmentType.AWSLocal)
                {
                    var credentialsService = serviceCollection.BuildServiceProvider().GetService<ICredentialsService>();
                    var credentials = credentialsService?.GetCredentials();
                    if (credentials != null)
                    {
                        ImmutableCredentials credentialsObject = credentials.GetCredentials();
                        var credentialOptions = new AwsCredentialsOptions
                        {
                            AccessKey = credentialsObject.AccessKey,
                            Secret = credentialsObject.SecretKey,
                            AwsRegion = "eu-west-2"
                        };
                        serviceCollection.AddOptions<AwsCredentialsOptions>();
                        serviceCollection.Configure<AwsCredentialsOptions>(options =>
                        {
                            options.AccessKey = credentialOptions.AccessKey;
                            options.Secret = credentialOptions.Secret;
                            options.AwsRegion = credentialOptions.AwsRegion;
                        });
                    }
                }
                else
                {
                    serviceCollection.Configure<SqsOptions>(configuration.GetSection(nameof(SqsOptions)));
                    serviceCollection.Configure<AwsCredentialsOptions>(configuration.GetSection(nameof(AwsCredentialsOptions)));
                }
            }
            else
            {

            }
            serviceCollection.AddSingleton(appSettings);
            serviceCollection.AddTransient<IMessageConsumerFactory, MessageConsumerFactory>();
            return serviceCollection;
        }

        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                    {
                    var response = context.Response;
                    response.ContentType = "application/json";


                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var excpetion = contextFeature?.Error;
                    if (excpetion != null)
                    { 
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;


                        await response.WriteAsync("Exception occured " + excpetion.Message);
                    }

                });
            });
            return app;
        }
    }
}

