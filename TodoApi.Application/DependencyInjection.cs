using System;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(DependencyInjection));
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            return serviceCollection;
        }
    }
}

