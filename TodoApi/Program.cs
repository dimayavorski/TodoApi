using TodoApi.Application;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Options;
using TodoApi.Infrastructure;

namespace TodoApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var appSettings = new AppSettings();

        builder.Configuration.AddJsonFile($"appsettings.{currentEnv}.json", true).AddUserSecrets(typeof(Program).Assembly);

        if(!builder.Environment.IsLocal()) 
        {
            builder.Configuration.AddKeyVaultServices();
        }

        if(!Enum.TryParse(typeof(EnvironmentType), currentEnv, true, out var environmentType)) {
            throw new ApplicationException("Cannot parse environment type");
        }
        appSettings.EnvironmentType = (EnvironmentType)environmentType;

        builder.Services.AddSingleton(appSettings);


      

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        

        //builder.Services.AddHostedService<HostedMessageConsumer>();

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(appSettings, builder.Configuration);


        


        var app = builder.Build();

        //Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {

        }
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.ConfigureExceptionHandler();

        app.MapControllers();

        app.Run();
    }

    
}

