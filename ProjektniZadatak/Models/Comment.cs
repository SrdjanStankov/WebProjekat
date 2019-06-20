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

        public string Text { get; set; }

        public int Rating { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}