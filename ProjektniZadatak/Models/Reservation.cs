using System;

namespace ProjektniZadatak.Models
{
    public class Reservation
    {
        public Reservation()
        {
        }

        public Reservation(Apartment reservedApartment, DateTime reservationStartDate, int numberOfNights, int totalPrice, Guest guest, ReservationStatus status)
        {
            ReservedApartment = reservedApartment;
            ReservationStartDate = reservationStartDate;
            NumberOfNights = numberOfNights;
            TotalPrice = totalPrice;
            Guest = guest;
            Status = status;
        }

        public int Id { get; set; }

        public Apartment ReservedApartment { get; set; }

        public DateTime ReservationStartDate { get; set; }

        public int NumberOfNights { get; set; } = 1;

        public int TotalPrice { get; set; }

        public Guest Guest { get; set; }

        public ReservationStatus Status { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}