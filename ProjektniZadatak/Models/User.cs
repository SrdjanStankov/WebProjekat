using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid last name")]
        public string Lastname { get; set; }

        [Column("Gender")]
        [Display(Name = "Gender")]
        public string GenderString { get { return Gender.ToString(); } set { Gender = (Genders)Enum.Parse(typeof(Genders), value, true); } }

        [NotMapped]
        public Genders Gender { get; set; }
    }
}