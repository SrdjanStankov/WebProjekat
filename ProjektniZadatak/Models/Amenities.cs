using System.ComponentModel.DataAnnotations;

namespace ProjektniZadatak.Models
{
    public class Amenities
    {
        public Amenities()
        {
        }

        public Amenities(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.", AllowEmptyStrings = false)]
        public string Name { get; set; }

        public Apartment Apartment_Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}