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
    public class HotelsRepository : IHotelsRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ApplicationContext _context;

        public HotelsRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
        }

        public async Task<int> Create(Hotels item)
        {
            await _context.Hotels.AddAsync(item);
            _context.SaveChanges();

            return item.Id;
        }

        public async Task<bool> Edit(Hotels item)
        {
            bool res = false;

            Hotels hotel = await _context.Hotels.FindAsync(item.Id);
            if (hotel != null)
            {
                hotel.Name = item.Name;
                hotel.Address = item.Address;
                hotel.StarRating = item.StarRating;
                hotel.CreatedAt = item.CreatedAt;
                hotel.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                res = true;
            }

            return res;
        }

        public Hotels GetById(int id)
        {
            return _context.Hotels.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<Hotels> GetList()
        {
            return _context.Hotels.AsNoTracking().ToList();
        }
    }
}