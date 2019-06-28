namespace ProjektniZadatak.Models
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string street, string number, string town, string zipCode)
        {
            Street = street;
            Number = number;
            Town = town;
            ZipCode = zipCode;
        }

        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Town { get; set; }

        public string ZipCode { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}