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

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Amenities> Amenities { get; set; }


        public Model() : base("Agency")
        {
            Database.CreateIfNotExists();
        }

        #region Reservation Methods

        public void AddReservation(Reservation reservation)
        {
            Apartments.Attach(reservation.ReservedApartment);
            Users.Attach(reservation.Guest);
            Reservations.Add(reservation);
            SaveChanges();
        }

        public void RemoveReservation(Reservation reservation)
        {
            Reservations.Attach(reservation);
            Reservations.Remove(reservation);
            SaveChanges();
        }

        public IEnumerable<Reservation> GetReservations()
        {
            return Reservations.AsNoTracking().Select(s => s);
        }

        public IEnumerable<Reservation> GetReservations(int apartmentid)
        {
            return Reservations.AsNoTracking().Where(i => i.ReservedApartment.Id == apartmentid);
        }

        public Reservation GetReservation(int id)
        {
            return Reservations.AsNoTracking().Where(i => i.Id == id).First();
        }

        #endregion

        #region Comments Methods

        public void AddComment(Comment comment)
        {
            Users.Attach(comment.GuestThaWroteComment);
            Apartments.Attach(comment.Apartment);
            Comments.Add(comment);
            SaveChanges();
        }

        public void RemoveComment(Comment comment)
        {
            Comments.Attach(comment);
            Comments.Remove(comment);
            SaveChanges();
        }

        public IEnumerable<Comment> GetComments()
        {
            return Comments.AsNoTracking().Select(s => s);
        }

        public IEnumerable<Comment> GetComments(int apartmentId)
        {
            return Comments.AsNoTracking().Where(i => i.Apartment.Id == apartmentId);
        }

        public Comment GetComment(int id)
        {
            return Comments.AsNoTracking().Where(s => s.Id == id).First();
        }

        #endregion

        #region Location Methods

        public void AddLocation(Location location)
        {
            Addresses.Attach(location.Address);
            Locations.Add(location);
            SaveChanges();
        }

        public IEnumerable<Location> GetLocations()
        {
            return Locations.AsNoTracking().Select(s => s);
        }

        public IEnumerable<Location> GetLocations(int id)
        {
            return Locations.AsNoTracking().Where(s => s.Id == id);
        }

        public Location GetLocation(int id)
        {
            return Locations.AsNoTracking().Where(s => s.Id == id).First();
        }

        #endregion

        #region Address Methods

        public void AddAddress(Address address)
        {
            Addresses.Add(address);
            SaveChanges();
        }

        public void RemoveAddress(Address address)
        {
            Addresses.Attach(address);
            Addresses.Remove(address);
            SaveChanges();
        }

        public IEnumerable<Address> GetAddresses()
        {
            return Addresses.AsNoTracking().Select(s => s);
        }

        public IEnumerable<Address> GetAddresses(int id)
        {
            return Addresses.AsNoTracking().Where(s => s.Id == id);
        }

        public Address GetAddress(int id)
        {
            return Addresses.AsNoTracking().Where(s => s.Id == id).First();
        }

        #endregion

        #region Amenities Methods

        public void AddAmmenitie(Amenities amenities)
        {
            Amenities.Add(amenities);
            SaveChanges();
        }

        public void RemoveAmenitie(Amenities amenities)
        {
            Amenities.Attach(amenities);
            Amenities.Remove(amenities);
            SaveChanges();
        }

        public IEnumerable<Amenities> GetAmenities()
        {
            return Amenities.AsNoTracking().Select(s => s);
        }

        public IEnumerable<Amenities> GetAmenities(int apartmentId)
        {
            return Amenities.AsNoTracking().Where(s => s.Apartment_Id.Id == apartmentId);
        }

        public Amenities GetAmenitie(int id)
        {
            return Amenities.AsNoTracking().Where(s => s.Id == id).First();
        }

        #endregion

        #region Apartment Methods

        public void AddApartment(Apartment apartment)
        {
            Users.Attach(apartment.Host);
            Apartments.Add(apartment);
            SaveChanges();
        }

        public void RemoveApartment(Apartment apartment)
        {
            Apartments.Attach(apartment);
            Apartments.Remove(apartment);
            SaveChanges();
        }

        public IEnumerable<Apartment> GetApartments()
        {
            var apartments = Apartments.AsNoTracking().Select(ap => ap);
            FillApartmentsLists(apartments);
            return apartments;
        }

        public IEnumerable<Apartment> GetApartments(ApartmentStatus status)
        {
            var apartments = Apartments.AsNoTracking().Where(item => item.Status == status);
            FillApartmentsLists(apartments);
            return apartments;
        }

        public IEnumerable<Apartment> GetApartments(string hostUsername)
        {
            var apartments = Apartments.AsNoTracking().Where(item => item.Host.Username == hostUsername);
            FillApartmentsLists(apartments);
            return apartments;
        }

        public Apartment GetApartment(int id)
        {
            var apartment = Apartments.AsNoTracking().Where(s => s.Id == id).FirstOrDefault();
            apartment.Comments.AddRange(GetComments(apartment.Id));
            apartment.Reservations.AddRange(GetReservations(apartment.Id));
            apartment.Amenities.AddRange(GetAmenities(apartment.Id));
            return apartment;
        }

        private void FillApartmentsLists(IQueryable<Apartment> apartments)
        {
            foreach (var item in apartments)
            {
                item.Comments.AddRange(GetComments(item.Id));
                item.Reservations.AddRange(GetReservations(item.Id));
                item.Amenities.AddRange(GetAmenities(item.Id));
            }
        }

        #endregion

        #region User Methods

        public void AddUser(User user)
        {
            switch (user)
            {
                case Administrator _:
                    Users.Add(user as Administrator);
                    break;
                case Guest _:
                    Users.Add(user as Guest);
                    break;
                case Host _:
                    Users.Add(user as Host);
                    break;
                default:
                    Users.Add(user);
                    break;
            }
            SaveChanges();
        }

        public void RemoveUser(User user)
        {
            Users.Attach(user);
            var apartments = Apartments.AsNoTracking().Where(a => a.Host.Username == user.Username).ToList();
            foreach (var item in apartments)
            {
                RemoveApartment(item);
            }
            Users.Remove(user);
            SaveChanges();
        }

        public void RemoveUser(string username)
        {
            var user = GetUser(username);
            Users.Attach(user);
            var apartments = Apartments.AsNoTracking().Where(a => a.Host.Username == user.Username).ToList();
            foreach (var item in apartments)
            {
                RemoveApartment(item);
            }
            Users.Remove(user);
            SaveChanges();
        }

        public bool Exists(User user)
        {
            var usernames = GetUsers(user.Username).Select(u=>u.Username);

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

        public IEnumerable<User> GetUsers()
        {
            return Users.AsNoTracking().Select(user => user);
        }

        public IEnumerable<User> GetUsers(string username)
        {
            return Users.AsNoTracking().Where(user => user.Username  == username);
        }

        public User GetUser(string username)
        {
            return Users.AsNoTracking().Where(s => s.Username == username).FirstOrDefault();
        }

        #endregion
    }
}