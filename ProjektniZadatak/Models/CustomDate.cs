using System;
using System.Collections.Generic;

namespace ProjektniZadatak.Models
{
    public class CustomDate
    {
        public CustomDate()
        {
        }

        public CustomDate(DateTime date)
        {
            Date = date;
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<Apartment> Apartment_Id { get; set; }

        public virtual ICollection<Apartment> Apartment_Id1 { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}