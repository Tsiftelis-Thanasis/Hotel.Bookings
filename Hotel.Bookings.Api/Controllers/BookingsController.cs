using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Models;
using HotelBookings.Contract.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookings.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookingsController : ControllerBase
    {
        private ILogger<BookingsController> _logger;
        private readonly IBookingsStore _store;
        private readonly ICachingBookings _cachingBookings;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IHotelsRepository _hotelsRepository;

        public BookingsController(ILogger<BookingsController> logger, IBookingsStore store, ICachingBookings cachingBookings,
            IBookingsRepository bookingsRepository, IHotelsRepository hotelsRepository)
        {
            _store = store;
            _cachingBookings = cachingBookings;
            _logger = logger;
            _bookingsRepository = bookingsRepository;
            _hotelsRepository = hotelsRepository;
        }

        [HttpGet]
        public BookingsDto Get(int Id)
        {
            BookingsDto res = new BookingsDto();

            bool found = false;
            try
            {
                if (_cachingBookings.BookingsDto != null && _cachingBookings.BookingsDto.Any(x => x.Id.Equals(Id)))
                {
                    res = _cachingBookings.BookingsDto.Where(x => x.Id.Equals(Id)).First();
                    found = true;
                }
                else if (!found && _store.Store.ContainsKey(Id))
                {
                    res = _store.Store[Id];
                    _cachingBookings.AddBooking(res);
                    found = true;
                }
                else
                {
                    var booking = _bookingsRepository.GetById(Id);
                    if (booking != null)
                    {
                        res = booking.ToBookingDTO();
                        var hotel = _hotelsRepository.GetById(booking.HotelId);
                        if (hotel != null)
                            res.Hotel = hotel.ToHotelDTO();
                    }

                    if (res != null)
                    {
                        _store.AddBooking(res);
                        _cachingBookings.AddBooking(res);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return res;
            }

            return res;
        }

        [HttpGet]
        public List<BookingsDto> GetAll()
        {
            List<BookingsDto> res = new List<BookingsDto>();

            try
            {
                var bookings = _bookingsRepository.GetList();
                var hotels = _hotelsRepository.GetList();
                foreach (Bookings bookings1 in bookings)
                {
                    var hotel = hotels.Where(x => x.Id.Equals(bookings1.HotelId)).FirstOrDefault();
                    if (hotel != null)
                    {
                        BookingsDto bookingsDto = bookings1.ToBookingDTO();
                        bookingsDto.Hotel = hotel.ToHotelDTO();
                        res.Add(bookingsDto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return res;
            }

            return res;
        }

        [HttpPut]
        public async Task<bool> Edit(int bookingId, string customerName, int hotelId, int numberOfPax)
        {
            bool res = false;

            try
            {
                Bookings booking = new Bookings()
                {
                    Id = bookingId,
                    CustomerName = customerName,
                    HotelId = hotelId,
                    NumOfPax = numberOfPax,
                    UpdatedAt = DateTime.Now
                };

                res = await _bookingsRepository.Edit(booking);

                if (res)
                {
                    _store.Update();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return res;
            }

            return res;
        }

        [HttpPost]
        public async Task<int> Create(string customerName, int hotelId, int numberOfPax)
        {
            int res = 0;

            try
            {
                Bookings booking = new Bookings()
                {
                    CustomerName = customerName,
                    HotelId = hotelId,
                    CreatedAt = DateTime.Now,
                    NumOfPax = numberOfPax
                };

                res = await _bookingsRepository.Create(booking);

                if (res > 0)
                {
                    _store.Update();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return res;
            }

            return res;
        }
    }
}