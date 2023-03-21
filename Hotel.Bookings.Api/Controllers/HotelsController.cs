using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Models;
using HotelBookings.Contract.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelHotels.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HotelsController : ControllerBase
    {
        private ILogger<HotelsController> _logger;
        private readonly IBookingsStore _store;
        private readonly ICachingBookings _cachingBookings;
        private readonly IHotelsRepository _hotelsRepository;

        public HotelsController(ILogger<HotelsController> logger, IBookingsStore store, ICachingBookings cachingBookings,
            IHotelsRepository hotelsRepository)
        {
            _store = store;
            _cachingBookings = cachingBookings;
            _logger = logger;
            _hotelsRepository = hotelsRepository;
        }

        [HttpGet]
        public HotelsDto Get(int Id)
        {
            HotelsDto res = new HotelsDto();

            bool found = false;
            try
            {
                if (_cachingBookings.BookingsDto != null && _cachingBookings.BookingsDto.Any(x => x.Hotel.Id.Equals(Id)))
                {
                    res = _cachingBookings.BookingsDto.Where(x => x.Hotel.Id.Equals(Id)).First().Hotel;
                    found = true;
                }
                else if (!found && _store.Store.Any(x => x.Value.Hotel.Id.Equals(Id)))
                {
                    var bookings = _store.Store.Where(x => x.Value.Hotel.Id.Equals(Id)).FirstOrDefault();
                    _cachingBookings.AddBooking(bookings.Value);
                    res = bookings.Value.Hotel;
                    found = true;
                }
                else
                {
                    var hotel = _hotelsRepository.GetById(Id);
                    if (hotel != null)
                    {
                        res = hotel.ToHotelDTO();
                    }

                    //if (res != null)
                    //{
                    //    _store.AddBooking(res);
                    //    _cachingHotels.AddBooking(res);
                    //}
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
        public List<HotelsDto> GetAll()
        {
            List<HotelsDto> res = new List<HotelsDto>();

            try
            {
                var hotels = _hotelsRepository.GetList();
                foreach (Hotels hotel in hotels)
                {
                    res.Add(hotel.ToHotelDTO());
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
        public async Task<bool> Edit(int hotelId, string name, string address)
        {
            bool res = false;

            try
            {
                Hotels hotel = new Hotels()
                {
                    Id = hotelId,
                    Name = name,
                    Address = address,

                    UpdatedAt = DateTime.Now
                };

                res = await _hotelsRepository.Edit(hotel);

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
        public async Task<int> Create(string name, string address)
        {
            int res = 0;

            try
            {
                Hotels hotel = new Hotels()
                {
                    Name = name,
                    Address = address,
                    CreatedAt = DateTime.Now
                };

                res = await _hotelsRepository.Create(hotel);

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