using HotelBookings.Application.Interfaces;
using HotelBookings.Contract.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace HotelBookings.Application.Services
{
    public class CachingBookings : ICachingBookings
    {
        public ConcurrentQueue<BookingsDto> BookingsDto { get; set; }
        public DateTime LastUpdate { get; set; }
        private bool _timeHasPassed;
        private int _startTime;

        public CachingBookings(IServiceScopeFactory scopeFactory)
        {
            _timeHasPassed = true;
            _startTime = Environment.TickCount;
            _timeHasPassed = (Environment.TickCount - _startTime > 10000);
        }

        public void AddBooking(BookingsDto booking)
        {
            if (BookingsDto == null)
            {
                BookingsDto = new ConcurrentQueue<BookingsDto>();
                BookingsDto.Enqueue(booking);
                LastUpdate = DateTime.Now;
            }
            else
            {
                BookingsDto.Enqueue(booking);
                LastUpdate = DateTime.Now;
            }
        }
    }
}