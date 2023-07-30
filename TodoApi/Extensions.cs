using System.Net;
using Amazon.Runtime;
using Microsoft.AspNetCore.Diagnostics;
using TodoApi.Application.Common.Enums;
using TodoApi.Application.Common.Interfaces;
using TodoApi.Application.Common.Options;
using TodoApi.Application.Common.Options.Aws;
using TodoApi.Infrastructure.MessageConsumer;
using TodoApi.Infrastructure.Secrets;

namespace TodoApi
{
    public static class Extensions
    {
        public static bool IsLocal(this IWebHostEnvironment env)
        {
            return env.IsEnvironment(EnvironmentType.AzureLocal.ToString()) || env.IsEnvironment(EnvironmentType.AWSLocal.ToString());
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

