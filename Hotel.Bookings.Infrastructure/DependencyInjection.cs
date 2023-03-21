using HotelBookings.Application.BackgroundServices;
using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HotelBookings.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContosoServices(this IServiceCollection serviceCollection)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton<IBookingsRepository, BookingsRepository>();
            serviceCollection.AddSingleton<IHotelsRepository, HotelsRepository>();
            serviceCollection.AddSingleton<ICachingBookings, CachingBookings>();
            serviceCollection.AddSingleton<IBookingsStore, BookingStore>();
            serviceCollection.AddTransient(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            serviceCollection.AddSingleton<IHostedService, DBInfoUpdater>();

            return serviceCollection;
        }
    }
}