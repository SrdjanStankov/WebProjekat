using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjektniZadatak.Models
{
    [System.Serializable]
    public class User
    {
        [Required]
        [System.Xml.Serialization.XmlElement]
        public string Username { get; set; }

        [Required]
        [System.Xml.Serialization.XmlElement]
        public string Password { get; set; }

        [Required]
        [System.Xml.Serialization.XmlElement]
        public string Name { get; set; }

        [Required]
        [System.Xml.Serialization.XmlElement]
        public string Lastname { get; set; }

        [Required]
        [System.Xml.Serialization.XmlElement]
        public Genders Gender { get; set; }

        [System.Xml.Serialization.XmlElement]
        public Role Role { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<Apartment> ApartmentsForRent { get; set; } = new List<Apartment>();

        [System.Xml.Serialization.XmlIgnore]
        public List<Apartment> RentedApartments { get; set; } = new List<Apartment>();

        [System.Xml.Serialization.XmlIgnore]
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}