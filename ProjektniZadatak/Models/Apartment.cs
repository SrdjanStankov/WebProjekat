using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjektniZadatak.Models
{
    public class Apartment
    {
        public int Id { get; set; }

        public ApartmentType ApartmentType { get; set; }

        [Required]
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Number of rooms must be greater than 0")]
        public int NumberOfRooms { get; set; }

        [Required]
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Number of guests must be greater than 0")]
        public int NumberOfGuests { get; set; }

        public Location Location { get; set; }

        public List<DateTime> DatesForIssues { get; set; } = new List<DateTime>();

        public List<DateTime> AvailableDates { get; set; } = new List<DateTime>();

        public Host Host { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        // pictures

        [Required]
        [DataType(DataType.Currency)]
        public int PricePerNight { get; set; }

        [DataType(DataType.Time)]
        public DateTime TimeOfRegistration { get; set; } = new DateTime(2019, 1, 1, 14, 0, 0);

        [DataType(DataType.Time)]
        public DateTime TimeOfCheckOut { get; set; } = new DateTime(2019, 1, 1, 10, 0, 0);

        public ApartmentStatus Status { get; set; }

        public List<Amenities> Amenities { get; set; } = new List<Amenities>();

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}