using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjektniZadatak.Models.Databse
{
    public class Model : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Apartment> Apartments { get; set; }


        public Model() : base("Agency")
        {
            Database.CreateIfNotExists();
        }

        public List<Location> GetAllLocations()
        {
            return Locations.AsNoTracking().Select(s => s).ToList();
        }

        public void AddApartment(Apartment apartment)
        {
            Apartments.Add(apartment);
            SaveChanges();
        }

        public void RemoveApartment(Apartment apartment)
        {
            Apartments.Attach(apartment);
            Apartments.Remove(apartment);
            SaveChanges();
        }

        public List<Apartment> GetAllApartmets()
        {
            return Apartments.AsNoTracking().Select(ap => ap).ToList();
        }

        public Apartment GetApartment(int id)
        {
            return Apartments.AsNoTracking().Select(ap => ap).Where(s => s.Id == id).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            if (user is Administrator)
            {
                Users.Add(user as Administrator);
            }
            else if (user is Guest)
            {
                Users.Add(user as Guest);
            }
            else if (user is Host)
            {
                Users.Add(user as Host);
            }
            else
            {
                Users.Add(user);
            }
            SaveChanges();
        }

        public void RemoveUser(User user)
        {
            Users.Attach(user);
            Users.Remove(user);
            SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return Users.AsNoTracking().Select(user => user).ToList();
        }

        public bool Exists(User user)
        {
            var users = GetAllUsers();

            var usernames = users.Select(u => u.Username).ToList();

            foreach (string item in usernames)
            {
                if (item == user.Username)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ReplaceUser(User from, User to)
        {
            if (Users.AsNoTracking().Select(u => u.Username == to.Username).FirstOrDefault())
            {
                return false;
            }

            RemoveUser(from);
            AddUser(to);
            return true;
        }

        public System.Data.Entity.DbSet<ProjektniZadatak.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<ProjektniZadatak.Models.Location> Locations { get; set; }

        public System.Data.Entity.DbSet<ProjektniZadatak.Models.Address> Addresses { get; set; }

        public User GetUser(string username)
        {
            return Users.AsNoTracking().Select(all => all).Where(s => s.Username == username).FirstOrDefault();
        }
    }
}