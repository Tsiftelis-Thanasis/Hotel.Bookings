using HotelBookings.Contract.Dtos;
using HotelBookings.Contract.Enums;
using System;

namespace HotelBookings.Application.Models
{
    public class Hotels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int StarRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public HotelsDto ToHotelDTO()
        {
            return new HotelsDto
            {
                Id = Id,
                Name = Name,
                Address = Address,
                StarRating = (Stars)StarRating,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }
    }
}