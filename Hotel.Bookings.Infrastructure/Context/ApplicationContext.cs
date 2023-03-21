using HotelBookings.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Application.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Bookings> Bookings { get; set; }

        public DbSet<Hotels> Hotels { get; set; }
    }
}