﻿namespace ProjektniZadatak.Models
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

        public Guest GuestThaWroteComment { get; set; }

        public Apartment Apartment { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }
    }
}