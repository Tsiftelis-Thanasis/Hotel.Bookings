using HotelBookings.Contract.Dtos;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace HotelBookings.Application.Interfaces
{
    public interface IBookingsStore
    {
        public ConcurrentDictionary<int, BookingsDto> Store { get; }

        Task<bool> AddBooking(BookingsDto bookingsDto);

        void Update();
    }
}