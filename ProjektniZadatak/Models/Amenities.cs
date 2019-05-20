namespace ProjektniZadatak.Models
{
    public class Amenities
    {
        public Amenities(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}