using System.Collections.Generic;

namespace ProjektniZadatak.Models
{
    public class User
    {
        public User(string username, string password, string name, string lastname, Genders gender, Role role)
        {
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
            Role = role;
            ApartmentsForRent = new List<Apartment>();
            RentedApartments = new List<Apartment>();
            Reservations = new List<Reservation>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public Genders Gender { get; set; }
        public Role Role { get; set; }
        public List<Apartment> ApartmentsForRent { get; set; }
        public List<Apartment> RentedApartments { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}