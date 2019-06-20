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

        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }

        public virtual Address Address { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}