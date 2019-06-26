using System.ComponentModel.DataAnnotations;

namespace ProjektniZadatak.Models
{
    public class Comment
    {
        public Comment()
        {
        }

        public Comment(Guest guestThaWroteComment, Apartment apartment, string text, int rating)
        {
            GuestThaWroteComment = guestThaWroteComment;
            Apartment = apartment;
            Text = text;
            Rating = rating;
        }

        public int Id { get; set; }

        public virtual Guest GuestThaWroteComment { get; set; }

        public virtual Apartment Apartment { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Text of comment is required")]
        public string Text { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Rating of comment is required")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public int Rating { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool Show { get; set; } = false;
    }
}