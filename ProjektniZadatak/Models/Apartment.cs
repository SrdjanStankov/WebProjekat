using System;
using System.Collections.Generic;

namespace ProjektniZadatak.Models
{
    public class Apartment
    {
        public Apartment(ApartmentType apartmentType, int numberOfRooms, int numberOfGuests, Location location, User host, string komentari, int pricePerNight, DateTime timeOfRegistration, DateTime timeOfCheckOut, ApartmentStatus status)
        {
            ApartmentType = apartmentType;
            NumberOfRooms = numberOfRooms;
            NumberOfGuests = numberOfGuests;
            Location = location;
            Host = host;
            Komentari = komentari;
            PricePerNight = pricePerNight;
            TimeOfRegistration = timeOfRegistration;
            TimeOfCheckOut = timeOfCheckOut;
            Status = status;
            DatesForIssue = new List<DateTime>();
            AvailableDates = new List<DateTime>();
            Amenities = new List<Amenities>();
            Reservations = new List<Reservation>();
        }

        public ApartmentType ApartmentType { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfGuests { get; set; }
        public Location Location { get; set; }
        public List<DateTime> DatesForIssue { get; set; }
        public List<DateTime> AvailableDates { get; set; }
        public User Host { get; set; }
        public string Komentari { get; set; }
        // pictures
        public int PricePerNight { get; set; }
        public DateTime TimeOfRegistration { get; set; }
        public DateTime TimeOfCheckOut { get; set; }
        public ApartmentStatus Status { get; set; }
        public List<Amenities> Amenities { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}