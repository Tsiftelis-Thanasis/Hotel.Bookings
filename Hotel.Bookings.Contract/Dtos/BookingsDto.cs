using System;

namespace HotelBookings.Contract.Dtos
{
    public class BookingsDto
    {
        public int Id { get; set; }
        public HotelsDto Hotel { get; set; }
        public string CustomerName { get; set; }
        public int NumOfPax { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}