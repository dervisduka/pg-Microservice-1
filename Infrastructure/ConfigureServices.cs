using Application.Options;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConnectionStringsOptions connectionStrings, RedisOptions redis, IConfiguration configuration, MessageBrokerOptions mbOptions)
        {

            services.AddInfrastructure(configuration);

            if (bool.Parse(configuration["UseRedis"] ?? "true"))
            {
                services.AddStackExchangeRedisCache(option =>
                {
                    option.Configuration = connectionStrings.RedisConnection;
                    option.InstanceName = redis.InstanceName;
                });
            }

            if (bool.Parse(configuration["UseMassTransit"] ?? "true"))
            {
                services.AddMassTransit(busConfigurator =>
                {
                    busConfigurator.UsingRabbitMq((context, configurator) =>
                    {
                        configurator.Host(new Uri(mbOptions.Host!), h =>
                        {
                            h.Username(mbOptions.Username);
                            h.Password(mbOptions.Password);
                        });

                        configurator.ConfigureEndpoints(context);
                    });
                });
            }

            return services;
        }
    }
}
