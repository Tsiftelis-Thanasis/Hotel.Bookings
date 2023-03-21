using HotelBookings.Application.Context;
using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Models;
using HotelBookings.Contract.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookings.Application.BackgroundServices
{
    public class DBInfoUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ApplicationContext _context;
        
        private bool _timeHasPassed;
        private int _startTime;

        private double _lasttick = 0;
        private double _lastUpdate = 0;
        private double _updateTick = 10000;

        public DBInfoUpdater(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
            _lasttick = 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _lasttick = Environment.TickCount;
                if (_lasttick >= _lastUpdate + _updateTick)
                {
                    _lastUpdate = _lasttick;
                    await AsyncUpdate();
                }
            }
        }

        protected async Task AsyncUpdate()
        {
            try
            {
                // possible update of the database if we have data in another store.               
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                await Task.Delay(1);
            }
        }
    }
}