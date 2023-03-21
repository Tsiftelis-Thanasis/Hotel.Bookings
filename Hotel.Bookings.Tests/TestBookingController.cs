using HotelBookings.Application.Interfaces;
using HotelBookings.Contract.Dtos;
using HotelHotels.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace HotelBookings.Api.Controllers
{
    public class TestBookingController : Controller
    {
        private ILogger<BookingsController> _logger;
        private ILogger<HotelsController> _hotelsLogger;

        private readonly IBookingsStore _store;
        private readonly ICachingBookings _cachingBookings;

        private readonly IBookingsRepository _bookingsRepository;
        private readonly IHotelsRepository _hotelsRepository;

        private readonly BookingsController _bookings;
        private readonly HotelsController _hotels;

        public TestBookingController(ILogger<BookingsController> logger, ILogger<HotelsController> hotelsLogger
            , IBookingsStore store, ICachingBookings cachingBookings
            , IBookingsRepository bookingsRepository, IHotelsRepository hotelsRepository)
        {
            _store = store;
            _cachingBookings = cachingBookings;
            _logger = logger;
            _hotelsLogger = hotelsLogger;
            _bookingsRepository = bookingsRepository;
            _hotelsRepository = hotelsRepository;
            _bookings = new BookingsController(_logger, _store, _cachingBookings, _bookingsRepository, _hotelsRepository);
            _hotels = new HotelsController(_hotelsLogger, _store, _cachingBookings, _hotelsRepository);
        }

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Theory]
        public void CheckBooking(int id)
        {
            var booking = Booking();

            var result = _bookings.Get(id);

            Assert.Equal(booking, result);
        }

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Theory]
        public void CheckHotel(int id)
        {
            var hotel = Hotel();

            var result = _hotels.Get(id);

            Assert.Equal(hotel, result);
        }

        private HotelsDto Hotel()
        {
            return new HotelsDto()
            {
                Id = 1,
                Name = "Greece",
                Address = "GR",
                StarRating = 0,
                CreatedAt = DateTime.Now
            };
        }

        private BookingsDto Booking()
        {
            return new BookingsDto()
            {
                Id = 10,
                CustomerName = "Thanasis",
                Hotel = Hotel(),
                NumOfPax = 4,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }
    }
}