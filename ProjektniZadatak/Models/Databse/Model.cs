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
            var res = Reservations.SingleOrDefault(i => i.Id == reservation.Id);
            res.IsDeleted = true;
            //Reservations.Attach(reservation);
            //Reservations.Remove(reservation);
            SaveChanges();
        }

        public IEnumerable<Reservation> GetReservations()
        {
            return Reservations.AsNoTracking().Where(i=> i.IsDeleted == false);
        }

        public IEnumerable<Reservation> GetReservations(int apartmentid)
        {
            return Reservations.AsNoTracking().Where(i => i.ReservedApartment.Id == apartmentid && i.IsDeleted == false);
        }

        public Reservation GetReservation(int id)
        {
            return Reservations.AsNoTracking().Where(i => i.Id == id && i.IsDeleted == false).First();
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
            var comm = Comments.SingleOrDefault(i => i.Id == comment.Id);
            comm.IsDeleted = true;
            //Comments.Attach(comment);
            //Comments.Remove(comment);
            SaveChanges();
        }

        public IEnumerable<Comment> GetComments()
        {
            return Comments.AsNoTracking().Where(i => i.IsDeleted == false);
        }

        public IEnumerable<Comment> GetComments(int apartmentId)
        {
            return Comments.AsNoTracking().Where(i => i.Apartment.Id == apartmentId && i.IsDeleted == false);
        }

        public Comment GetComment(int id)
        {
            return Comments.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false).First();
        }

        #endregion

        #region Location Methods

        public void AddLocation(Location location)
        {
            Addresses.Attach(location.Address);
            Locations.Add(location);
            SaveChanges();
        }

        public void RemoveLocation(Location location)
        {
            var loc = Locations.SingleOrDefault(i => i.Id == location.Id);
            loc.IsDeleted = true;
            SaveChanges();
        }

        public IEnumerable<Location> GetLocations()
        {
            return Locations.AsNoTracking().Where(s => s.IsDeleted == false);
        }

        public IEnumerable<Location> GetLocations(int id)
        {
            return Locations.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false);
        }

        public Location GetLocation(int id)
        {
            return Locations.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false).First();
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
            var addr = Addresses.SingleOrDefault(s => s.Id == address.Id);
            addr.IsDeleted = true;
            //Addresses.Attach(address);
            //Addresses.Remove(address);
            SaveChanges();
        }

        public IEnumerable<Address> GetAddresses()
        {
            return Addresses.AsNoTracking().Where(s => s.IsDeleted == false);
        }

        public IEnumerable<Address> GetAddresses(int id)
        {
            return Addresses.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false);
        }

        public Address GetAddress(int id)
        {
            return Addresses.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false).First();
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
            var amm = Amenities.SingleOrDefault(s => s.Id == amenities.Id);
            amm.IsDeleted = true;
            //Amenities.Attach(amenities);
            //Amenities.Remove(amenities);
            SaveChanges();
        }

        public IEnumerable<Amenities> GetAmenities()
        {
            return Amenities.AsNoTracking().Where(s => s.IsDeleted == false);
        }

        public IEnumerable<Amenities> GetAmenities(int apartmentId)
        {
            return Amenities.AsNoTracking().Where(s => s.Apartment_Id.Id == apartmentId && s.IsDeleted == false);
        }

        public Amenities GetAmenitie(int id)
        {
            return Amenities.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false).First();
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
            var apa = Apartments.SingleOrDefault(s => s.Id == apartment.Id);
            apa.IsDeleted = true;
            //Apartments.Attach(apartment);
            //Apartments.Remove(apartment);
            SaveChanges();
        }

        public IEnumerable<Apartment> GetApartments()
        {
            var apartments = Apartments.AsNoTracking().Where(ap => ap.IsDeleted == false);
            FillApartmentsLists(apartments);
            return apartments;
        }

        public IEnumerable<Apartment> GetApartments(ApartmentStatus status)
        {
            var apartments = Apartments.AsNoTracking().Where(item => item.Status == status && item.IsDeleted == false);
            FillApartmentsLists(apartments);
            return apartments;
        }

        public IEnumerable<Apartment> GetApartments(string hostUsername)
        {
            var apartments = Apartments.AsNoTracking().Where(item => item.Host.Username == hostUsername && item.IsDeleted == false);
            FillApartmentsLists(apartments);
            return apartments;
        }

        public Apartment GetApartment(int id)
        {
            var apartment = Apartments.AsNoTracking().Where(s => s.Id == id && s.IsDeleted == false).FirstOrDefault();
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
            var us = Users.SingleOrDefault(i => i.Username == user.Username);

            var apartments = Apartments.Where(i => i.Host.Username == us.Username && i.IsDeleted == false);
            foreach (var item in apartments)
            {
                item.IsDeleted = true;
            }

            us.IsDeleted = true;

            //Users.Attach(user);
            //var apartments = Apartments.AsNoTracking().Where(a => a.Host.Username == user.Username).ToList();
            //foreach (var item in apartments)
            //{
            //    RemoveApartment(item);
            //}
            //Users.Remove(user);
            SaveChanges();
        }

        public void RemoveUser(string username)
        {
            var us = Users.SingleOrDefault(i => i.Username == username);

            var apartments = Apartments.Where(i => i.Host.Username == us.Username && i.IsDeleted == false);
            foreach (var item in apartments)
            {
                item.IsDeleted = true;
            }

            us.IsDeleted = true;
            //var user = GetUser(username);
            //Users.Attach(user);
            //var apartments = Apartments.AsNoTracking().Where(a => a.Host.Username == user.Username).ToList();
            //foreach (var item in apartments)
            //{
            //    RemoveApartment(item);
            //}
            //Users.Remove(user);
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
            return Users.AsNoTracking().Where(user => user.IsDeleted == false);
        }

        public IEnumerable<User> GetUsers(string username)
        {
            return Users.AsNoTracking().Where(user => user.Username  == username && user.IsDeleted == false);
        }

        public User GetUser(string username)
        {
            return Users.AsNoTracking().Where(s => s.Username == username && s.IsDeleted == false).FirstOrDefault();
        }

        #endregion
    }
}