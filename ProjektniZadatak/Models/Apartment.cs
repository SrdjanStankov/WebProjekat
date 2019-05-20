using System;
using System.Collections.Generic;

namespace ProjektniZadatak.Models
{
    public class Apartment
    {
        public ApartmentType ApartmentType { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfGuests { get; set; }
        public Location Location { get; set; }
        public List<DateTime> DatesForIssue { get; set; } = new List<DateTime>();
        public List<DateTime> AvailableDates { get; set; } = new List<DateTime>();
        public User Host { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        // pictures
        public int PricePerNight { get; set; }
        public DateTime TimeOfRegistration { get; set; }
        public DateTime TimeOfCheckOut { get; set; }
        public ApartmentStatus Status { get; set; }
        public List<Amenities> Amenities { get; set; } = new List<Amenities>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}