using HotelBookings.Contract.Enums;
using System;

namespace HotelBookings.Contract.Dtos
{
    public class HotelsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Stars StarRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}