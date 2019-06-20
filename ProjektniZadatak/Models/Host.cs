using System.Collections.Generic;

namespace ProjektniZadatak.Models
{
    public class Host : User
    {
        public Host()
        {
        }

        public Host(string username, string password, string name, string lastname, Genders gender) : base(username, password, name, lastname, gender)
        {
        }

        public virtual ICollection<Apartment> ApartmentsForRent { get; set; } = new List<Apartment>();
    }
}