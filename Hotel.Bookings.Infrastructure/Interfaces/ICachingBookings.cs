using HotelBookings.Contract.Dtos;
using System;
using System.Collections.Concurrent;

namespace HotelBookings.Application.Interfaces
{
    public interface ICachingBookings
    {
        ConcurrentQueue<BookingsDto> BookingsDto { get; set; }
        DateTime LastUpdate { get; set; }

        public void AddBooking(BookingsDto booking);
    }
}