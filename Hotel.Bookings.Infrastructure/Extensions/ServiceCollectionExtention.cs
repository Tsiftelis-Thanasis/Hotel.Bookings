using HotelBookings.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace HotelBookings.Application.Extensions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            var assmplyName = typeof(ServiceCollectionExtention).Assembly.FullName;

            services.AddServicesTransientEndsWith(assmplyName, "Repository");

            //services.AddScoped<IDapperFacadeFactory, DapperFacadeFactory>();

            //services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //services.AddTransient<IDbConnection>(db =>
            //            new SqlConnection(connectionString));

            //services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));

            services.AddScoped<Transaction>();

            services.AddTransient<IDbConnectionFactory>(db =>
                        new SqlConnectionFactory(connectionString));

            return services;
        }

        /* adding here so not to cause circural depedency */

        public static IServiceCollection AddServicesTransientEndsWith(this IServiceCollection services, string scannedAssemply, string endsWith)
        {
            Assembly assembly = Assembly.Load(scannedAssemply);
            var types = assembly.GetTypes().Where(x => !x.IsInterface && x.Name.EndsWith(endsWith));
            foreach (Type type in types)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Any())
                    services.AddTransient(interfaces.First(), type);
            }
            return services;
        }
    }
}