using System;

namespace ProjektniZadatak.Models
{
    public class IssueDates
    {
        public IssueDates()
        {
        }

        public IssueDates(DateTime issueDate, Apartment apartment)
        {
            IssueDate = issueDate;
            Apartment = apartment;
        }

        public int Id { get; set; }

        public DateTime IssueDate { get; set; }

        public int ApartmentId { get; set; }

        public Apartment Apartment { get; set; }
    }
}