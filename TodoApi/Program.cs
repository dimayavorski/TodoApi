using Microsoft.OpenApi.Models;
using TodoApi.Application;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Options;
using TodoApi.HostedServices;
using TodoApi.Infrastructure;

namespace TodoApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        using var serviceProvider = builder.Services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<Program>>()!;

        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var appSettings = new AppSettings();
        if (!Enum.TryParse(typeof(EnvironmentType), currentEnv, true, out var environmentType))
        {
            throw new ApplicationException("Cannot parse environment type");
        }
        appSettings.EnvironmentType = (EnvironmentType)environmentType;
        builder.Configuration.AddJsonFile($"appsettings.{currentEnv}.json", true).AddUserSecrets(typeof(Program).Assembly);

        logger.LogInformation($"Application configuration {currentEnv}");

        if (!builder.Environment.IsLocal())
        {
            if(appSettings.EnvironmentType == EnvironmentType.AWS)
            {
                builder.Configuration.AddAwsSecretsServices();
            }
            else
            {
                builder.Configuration.AddKeyVaultServices();
            }
        }

       
        builder.Services.AddSingleton(appSettings);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //builder.Services.AddHostedService<HostedMessageConsumer>();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(appSettings, builder.Configuration);

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.ConfigureExceptionHandler();
        app.MapControllers();

        app.Run();

    }


}

