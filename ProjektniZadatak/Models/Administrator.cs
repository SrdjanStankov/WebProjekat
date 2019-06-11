namespace ProjektniZadatak.Models
{
    public class Administrator : User
    {
        public Administrator()
        {
        }

        public Administrator(string username, string password, string name, string lastname, Genders gender) : base(username, password, name, lastname, gender)
        {
        }
    }
}