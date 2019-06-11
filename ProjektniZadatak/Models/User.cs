using System.ComponentModel.DataAnnotations;

namespace ProjektniZadatak.Models
{
    public class User
    {
        public User()
        {
        }

        public User(string username, string password, string name, string lastname, Genders gender)
        {
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid last name")]
        public string Lastname { get; set; }

        public Genders Gender { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}