using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Models;
using HotelHotels.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HotelBookings.Api.Controllers
{
    public class TestBookingController : Controller
    {
        private readonly IBookingsStore _store;

        private readonly ICachingBookings _cachingBookings;
        private readonly ILogger<BookingsController> _logger;
        private readonly ILogger<HotelsController> _hotelsLogger;
        private readonly Mock<IHotelsRepository> _hotelsRepository;

        //private readonly Mock<BookingsController> _bookingsController;
        private readonly Mock<IBookingsRepository> _bookingsRepository;

        public TestBookingController(IBookingsStore store, ICachingBookings caching, ILogger<BookingsController> logger,
            ILogger<HotelsController> hotelsLogger)
        {
            _store = store;
            _cachingBookings = caching;
            _logger = logger;
            _hotelsLogger = hotelsLogger;
            _hotelsRepository = new Mock<IHotelsRepository>();
            _bookingsRepository = new Mock<IBookingsRepository>();
            //_bookingsController = new Mock<BookingsController>();

        }


        //[Fact]
        //public void GetHotelId()
        //{
        //    var hotel = Hotel();

        //    _hotelsRepository.Setup(x => x.GetById(1)).Returns<Hotels>((Hotels h)=> hotel);

        //    HotelsController h = new HotelsController(_hotelsLogger, _store, _cachingBookings, _hotelsRepository.Object);
        //    var result = h.Get(1);

        //    Assert.Equal(hotel.ToHotelDTO(), result);
        //}

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Theory]
        public void CheckBooking(int id)
        {
            var booking = Booking();

            var res = _bookingsRepository.Setup(x => x.GetById(id)).Returns<Bookings>((Bookings x) => booking);



            Assert.NotNull(res);
            Assert.True(res.Equals(booking));
        }

        [Fact]
        public void CreateBooking()
        {
            var item = Booking();
            var id = 1;
            var res = _bookingsRepository.Setup(w => w.Create(item)).ReturnsAsync((int x) => id);
            Assert.True(res.Equals(1));
        }

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Theory]
        public void CheckHotel(int id)
        {
            var hotel = new Hotels();

            var result = _hotelsRepository.Setup(x => x.GetById(id)).Returns<Hotels>((Hotels x) => hotel);

            //Assert.True(!string.IsNullOrEmpty(hotel.Name));
        }

        [Fact]
        public void CreateHotel()
        {
            var item = Hotel();
            var res = 0;

            _hotelsRepository.Setup(w => w.Create(item)).ReturnsAsync((int x) => res);

            //Assert.True(res > 0);
        }

        private Hotels Hotel()
        {
            return new Hotels()
            {
                Id = 1,
                Name = "Greece",
                Address = "GR",
                StarRating = 0,
                CreatedAt = DateTime.Now
            };
        }

        private Bookings Booking()
        {
            return new Bookings()
            {
                Id = 3,
                CustomerName = "Thanasis",
                HotelId = 3,
                NumOfPax = 2,
                CreatedAt = new DateTime(2023, 1, 1)
            };
        }
    }
}