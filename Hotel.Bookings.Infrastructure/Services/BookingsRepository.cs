using HotelBookings.Application.Context;
using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookings.Application.Services
{
    public class BookingsRepository : IBookingsRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ApplicationContext _context;

        public BookingsRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
        }

        public async Task<int> Create(Bookings item)
        {
            await _context.Bookings.AddAsync(item);
            _context.SaveChanges();

            return item.Id;
        }

        public async Task<bool> Edit(Bookings item)
        {
            bool res = false;

            Bookings booking = await _context.Bookings.FindAsync(item.Id);
            if (booking != null)
            {
                booking.HotelId = item.HotelId;
                booking.CustomerName = item.CustomerName;
                booking.NumOfPax = item.NumOfPax;
                booking.CreatedAt = item.CreatedAt;
                booking.UpdatedAt = DateTime.Now;
                _context.SaveChanges();

                res = true;
            }

            return res;
        }

        public Bookings GetById(int id)
        {
            return _context.Bookings.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<Bookings> GetList()
        {
            return _context.Bookings.AsNoTracking().ToList();
        }
    }
}