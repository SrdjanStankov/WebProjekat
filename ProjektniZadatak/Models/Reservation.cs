using System;

namespace ProjektniZadatak.Models
{
    public class Reservation
    {
        public Reservation(Apartment reservedApartment, DateTime reservationStartDate, int numberOfNights, int totalPrice, User guest, ReservationStatus status)
        {
            ReservedApartment = reservedApartment;
            ReservationStartDate = reservationStartDate;
            NumberOfNights = numberOfNights;
            TotalPrice = totalPrice;
            Guest = guest;
            Status = status;
        }

        public Apartment ReservedApartment { get; set; }
        public DateTime ReservationStartDate { get; set; }
        public int NumberOfNights { get; set; }
        public int TotalPrice { get; set; }
        public User Guest { get; set; }
        public ReservationStatus Status { get; set; }
    }
}