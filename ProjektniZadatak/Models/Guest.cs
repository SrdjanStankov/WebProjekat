using System.Collections.Generic;

namespace ProjektniZadatak.Models
{
    public class Guest : User
    {
        public Guest()
        {
        }

        public Guest(string username, string password, string name, string lastname, Genders gender) : base(username, password, name, lastname, gender)
        {
        }

        public virtual ICollection<Apartment> RentedApartments { get; set; } = new List<Apartment>();

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}