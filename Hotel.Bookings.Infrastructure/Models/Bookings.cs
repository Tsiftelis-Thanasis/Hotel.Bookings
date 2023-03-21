using HotelBookings.Contract.Dtos;
using System;

namespace HotelBookings.Application.Models
{
    public class Bookings
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string CustomerName { get; set; }
        public int NumOfPax { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public BookingsDto ToBookingDTO()
        {
            return new BookingsDto
            {
                Id = Id,
                CustomerName = CustomerName,
                //Hotel
                NumOfPax = NumOfPax,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }
    }
}