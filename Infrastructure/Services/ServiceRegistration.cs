using Application.Common.Interfaces;
using Application.Options;
using Infrastructure.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlDataContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<PostgreDataContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ApplicationDbContextInitialiser>();

            // Register repositories
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbSession>();

            if (bool.Parse(configuration["UseMassTransit"] ?? "true"))
            {
                services.AddTransient<IBusBroker, BusBrokerContext>();
            }
            else
            {
                services.AddTransient<IBusBroker, NoOpBusBrokerContext>();
            }


            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProduct2Repository, Product2Repository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<IAlcoholRepository, AlcoholRepository>();

        }
    }
}
