using HotelBookings.Application.Interfaces;
using HotelBookings.Application.Models;
using HotelBookings.Contract.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookings.Application.Services
{
    public class BookingStore : IBookingsStore
    {
        public ConcurrentDictionary<int, BookingsDto> Store { get; set; }

        private IBookingsRepository _bookingsRepository;
        private IHotelsRepository _hotelsRepository;
        private List<Bookings> Bookings;
        private List<Hotels> Hotels;
        private bool _timeHasPassed;
        private int _startTime;

        public BookingStore(IBookingsRepository bookingsRepository, IHotelsRepository hotelsRepository)
        {
            _bookingsRepository = bookingsRepository;
            _hotelsRepository = hotelsRepository;
            _timeHasPassed = true;
            _startTime = Environment.TickCount;
            _timeHasPassed = (Environment.TickCount - _startTime > 10000);

            if (Bookings == null || _timeHasPassed)
                Bookings = _bookingsRepository.GetList();
            if (Hotels == null || _timeHasPassed)
                Hotels = _hotelsRepository.GetList();
            if (Store == null || _timeHasPassed)
                Store = GetStoreBookings();
        }

        private ConcurrentDictionary<int, BookingsDto> GetStoreBookings()
        {
            ConcurrentDictionary<int, BookingsDto> ids = new ConcurrentDictionary<int, BookingsDto>();

            foreach (var book in Bookings)
            {
                Hotels hotel = Hotels.Where(x => x.Id.Equals(book.HotelId)).FirstOrDefault();
                if (hotel != null)
                {
                    HotelsDto hotelDto = hotel.ToHotelDTO();
                    BookingsDto booking = book.ToBookingDTO();
                    booking.Hotel = hotelDto;
                    ids.TryAdd(book.Id, booking);
                }
            }

            return ids;
        }

        public async Task<bool> AddBooking(BookingsDto bookingsDto)
        {
            bool res = false;
            int hotelId = 0;
            int newbookingId = 0;

            if (Store.Count > 0)
                if (Store.ContainsKey(bookingsDto.Id))
                    RemoveBooking(bookingsDto.Id);

            if (!Hotels.Any(x => x.Id.Equals(bookingsDto.Hotel.Name)))
            {
                Hotels newhotel = new Hotels()
                {
                    Name = bookingsDto.Hotel.Name,
                    Address = bookingsDto.Hotel.Address,
                    StarRating = (int)bookingsDto.Hotel.StarRating,
                    CreatedAt = DateTime.Now
                };
                hotelId = await _hotelsRepository.Create(newhotel);
            }
            else
            {
                hotelId = bookingsDto.Hotel.Id;
            }

            if (!Bookings.Any(x => x.Id.Equals(bookingsDto.Id)))
            {
                Bookings newbooking = new Bookings()
                {
                    HotelId = hotelId,
                    CustomerName = bookingsDto.CustomerName,
                    NumOfPax = bookingsDto.NumOfPax,
                    CreatedAt = DateTime.Now
                };
                newbookingId = await _bookingsRepository.Create(newbooking);
            }
            else
            {
                newbookingId = bookingsDto.Id;
            }

            if (newbookingId > 0)
            {
                res = Store.TryAdd(newbookingId, bookingsDto);
            }

            return res;
        }

        public void Update()
        {
            Bookings = _bookingsRepository.GetList();
            Hotels = _hotelsRepository.GetList();
            Store = GetStoreBookings();
        }

        private void RemoveBooking(int id)
        {
            if (Store.ContainsKey(id))
            {
                Store.Remove(id, out _);
                if (Store.ContainsKey(id))
                {
                    Store.Remove(id, out _);
                }
            }
        }
    }
}