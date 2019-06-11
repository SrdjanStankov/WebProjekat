namespace ProjektniZadatak.Models
{
    public class Location
    {
        public Location()
        {
        }

        public Location(double latitude, double longitude, Address address)
        {
            Latitude = latitude;
            Longitude = longitude;
            Address = address;
        }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }

        public Address Address { get; set; }
    }
}