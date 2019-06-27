using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektniZadatak.Models
{
    public class Apartment
    {
        public int Id { get; set; }

        public ApartmentType ApartmentType { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Number of rooms must be greater than 0")]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Number of guests must be greater than 0")]
        public int NumberOfGuests { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<CustomDate> DatesForIssues { get; set; } = new List<CustomDate>();

        public virtual ICollection<CustomDate> AvailableDates { get; set; } = new List<CustomDate>();

        public virtual Host Host { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public string PicturesLocation { get; set; }

        public int PictureCount { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [DataType(DataType.Currency)]
        public int PricePerNight { get; set; }

        [DataType(DataType.Time)]
        public DateTime TimeOfRegistration { get; set; } = new DateTime(2019, 1, 1, 14, 0, 0);

        [DataType(DataType.Time)]
        public DateTime TimeOfCheckOut { get; set; } = new DateTime(2019, 1, 1, 10, 0, 0);

        public ApartmentStatus Status { get; set; }

        public virtual ICollection<Amenities> Amenities { get; set; } = new List<Amenities>();

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public bool IsDeleted { get; set; } = false;
    }
}