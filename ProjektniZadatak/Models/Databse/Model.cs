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
            reservation.ReservedApartment = Apartments.Where(a => a.IsDeleted == false && a.Id == reservation.ReservedApartment.Id).FirstOrDefault();
            reservation.Guest = Users.Where(u => u.IsDeleted == false && u.Username == reservation.Guest.Username).FirstOrDefault() as Guest;
            Reservations.Add(reservation);
            SaveChanges();
        }

        public void RemoveReservation(Reservation reservation)
        {
            var res = Reservations.SingleOrDefault(i => i.Id == reservation.Id);
            res.IsDeleted = true;
            SaveChanges();
        }

        public IEnumerable<Reservation> GetReservations(string guestUsername)
        {
            return Reservations.AsNoTracking().Where(i => i.IsDeleted == false && i.Guest.Username == guestUsername);
        }

        public IEnumerable<Reservation> GetReservations()
        {
            return Reservations.AsNoTracking().Where(i=> i.IsDeleted == false);
        }

        public IEnumerable<Reservation> GetReservations(int apartmentid)
        {
            return Reservations.AsNoTracking().Where(i => i.ReservedApartment.Id == apartmentid && i.IsDeleted == false);
        }

        public IEnumerable<Reservation> GetReservations(User Host)
        {
            return Reservations.AsNoTracking().Where(r => r.IsDeleted == false && r.ReservedApartment.Host.Username == Host.Username);
        }

        public Reservation GetReservation(int id)
        {
            return Reservations.AsNoTracking().Where(i => i.Id == id && i.IsDeleted == false).First();
        }

        public void ChangeReservationStatus(int id, ReservationStatus statusToChangeTo)
        {
            var reser = Reservations.Where(r => r.IsDeleted == false && r.Id == id).FirstOrDefault();
            reser.Status = statusToChangeTo;
            SaveChanges();
        }

        #endregion

        #region Comments Methods

        public void AddComment(Comment comment, int apartmentId)
        {
            var apartment = Apartments.Where(a => a.IsDeleted == false && a.Id == apartmentId).FirstOrDefault();
            var guest = Users.Where(u => u.IsDeleted == false && u.Username == comment.GuestThaWroteComment.Username).FirstOrDefault() as Guest;
            comment.GuestThaWroteComment = guest;
            comment.Apartment = apartment;
            Comments.Add(comment);
            SaveChanges();
        }

        public void RemoveComment(Comment comment)
        {
            var comm = Comments.SingleOrDefault(i => i.Id == comment.Id);
            comm.IsDeleted = true;
            SaveChanges();
        }

        public void SetShowOfComment(int id, bool show)
        {
            Comments.Where(c => c.IsDeleted == false && c.Id == id).FirstOrDefault().Show = true;
            SaveChanges();
        }

        public IEnumerable<Comment> GetComments()
        {
            return Comments.AsNoTracking().Include(i=>i.Apartment).Include(i=>i.GuestThaWroteComment).Where(i => i.IsDeleted == false);
        }

        public IEnumerable<Comment> GetComments(int apartmentId)
        {
            return Comments.AsNoTracking().Include(i => i.Apartment).Include(i => i.GuestThaWroteComment).Where(i => i.Apartment.Id == apartmentId && i.IsDeleted == false);
        }

        public IEnumerable<Comment> GetComments(int apartmentId, bool hostApproved)
        {
            return Comments.AsNoTracking().Include(i => i.Apartment).Include(i => i.GuestThaWroteComment).Where(i => i.Apartment.Id == apartmentId && i.IsDeleted == false && i.Show == hostApproved);
        }

        public Comment GetComment(int id)
        {
            return Comments.AsNoTracking().Include(i => i.Apartment).Include(i => i.GuestThaWroteComment).Where(s => s.Id == id && s.IsDeleted == false).First();
        }

        #endregion

        #region Location Methods

        public void AddLocation(Location location)
        {
            var adr = Addresses.Where(a => a.IsDeleted == false && a.Id == location.Address.Id).FirstOrDefault();
            location.Address = adr;
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
            return Locations.AsNoTracking().Include(i=>i.Address).Where(s => s.IsDeleted == false);
        }

        public IEnumerable<Location> GetLocations(int id)
        {
            return Locations.AsNoTracking().Include(i => i.Address).Where(s => s.Id == id && s.IsDeleted == false);
        }

        public Location GetLocation(int id)
        {
            return Locations.AsNoTracking().Include(i => i.Address).Where(s => s.Id == id && s.IsDeleted == false).First();
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

        public void AddAmenity(Amenities amenities)
        {
            Amenities.Add(amenities);
            SaveChanges();
        }

        public void RemoveAmenity(int amenityId)
        {
            var amm = Amenities.SingleOrDefault(s => s.Id == amenityId);
            amm.IsDeleted = true;
            //Amenities.Attach(amenities);
            //Amenities.Remove(amenities);
            SaveChanges();
        }

        public IEnumerable<Amenities> GetAmenities()
        {
            return Amenities.AsNoTracking().Include(i=>i.Apartments).Where(s => s.IsDeleted == false);
        }

        public Amenities GetAmenity(int id)
        {
            return Amenities.AsNoTracking().Include(i => i.Apartments).Where(s => s.Id == id && s.IsDeleted == false).First();
        }

        #endregion

        #region Apartment Methods

        public Apartment AddApartment(Apartment apartment)
        {
            var user = Users.Where(u => u.IsDeleted == false && u.Username == apartment.Host.Username).FirstOrDefault();
            apartment.Host = user as Host;

            ICollection<Amenities> amenities = new List<Amenities>();
            foreach (var item in apartment.Amenities)
            {
                amenities.Add(Amenities.SingleOrDefault(a => item.Id == a.Id));
            }
            apartment.Amenities = amenities;

            Apartments.Add(apartment);
            SaveChanges();
            var apartment1 = Apartments.Where(a => a.Id == apartment.Id).FirstOrDefault();
            apartment1.PicturesLocation += $"{apartment.Id}";
            SaveChanges();
            return apartment1;
        }

        public void EditApartment(Apartment apartment)
        {
            var apa = Apartments.SingleOrDefault(s => s.Id == apartment.Id);
            Entry(apa).CurrentValues.SetValues(apartment);
            apa.Amenities = new List<Amenities>();
            foreach (var item in apartment.Amenities)
            {
                apa.Amenities.Add(Amenities.SingleOrDefault(s => s.Id == item.Id));
            }
            SaveChanges();
        }

        public void RemoveApartment(int apartmentId)
        {
            var apa = Apartments.SingleOrDefault(s => s.Id == apartmentId);
            foreach (var item in Comments.Where(s=> s.Apartment.Id == apa.Id && s.IsDeleted == false))
            {
                item.IsDeleted = true;
            }
            apa.IsDeleted = true;
            SaveChanges();
        }

        public CustomDate FindMinDate(List<CustomDate> customDates)
        {
            return customDates.OrderBy(s => s.Date).FirstOrDefault();
        }

        public CustomDate FindMaxDate(List<CustomDate> customDates)
        {
            return customDates.OrderByDescending(s => s.Date).FirstOrDefault();
        }

        public IEnumerable<CustomDate> GetAvailableDates(int apartmentId)
        {
            return Apartments.AsNoTracking().Where(s => s.Id == apartmentId && s.IsDeleted == false).Select(i => i.AvailableDates).FirstOrDefault();
        }

        public IEnumerable<CustomDate> GetDatesForIssue(int apartmentId)
        {
            return Apartments.AsNoTracking().Where(s => s.Id == apartmentId && s.IsDeleted == false).Select(i => i.DatesForIssues).FirstOrDefault();
        }

        public IEnumerable<Apartment> GetApartments()
        {
            var apartments = Apartments.AsNoTracking().Include(s => s.AvailableDates).Include(s => s.DatesForIssues).Include(s => s.Comments).Include(r => r.Reservations).Include(h => h.Host).Include(a => a.Amenities).Include(l => l.Location).Where(ap => ap.IsDeleted == false);
            return apartments;
        }

        public IEnumerable<Apartment> GetApartments(ApartmentStatus status)
        {
            var apartments = Apartments.AsNoTracking().Include(s => s.AvailableDates).Include(s => s.DatesForIssues).Include(s=>s.Comments).Include(r=>r.Reservations).Include(h=>h.Host).Include(a=>a.Amenities).Include(l=>l.Location).Where(item => item.Status == status && item.IsDeleted == false);
            return apartments;
        }

        public IEnumerable<Apartment> GetApartments(string hostUsername)
        {
            var apartments = Apartments.AsNoTracking().Include(s => s.Comments).Include(r => r.Reservations).Include(s => s.AvailableDates).Include(s => s.DatesForIssues).Include(h => h.Host).Include(a => a.Amenities).Include(l => l.Location).Where(item => item.Host.Username == hostUsername && item.IsDeleted == false);
            return apartments;
        }

        public Apartment GetApartment(int id)
        {
            var apartment = Apartments.AsNoTracking().Include(s => s.AvailableDates).Include(s => s.DatesForIssues).Include(s => s.Comments).Include(r => r.Reservations).Include(h => h.Host).Include(a => a.Amenities).Include(l => l.Location).Where(s => s.Id == id && s.IsDeleted == false).FirstOrDefault();
            return apartment;
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
            SaveChanges();
        }

        public bool UsernameExists(User user)
        {
            var usernames =  Users.AsNoTracking().Select(u=>u.Username);

            foreach (var item in usernames)
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

        public List<DateTime> GetDatesNotActiveInRange(DateTime min, DateTime max, List<CustomDate> availableDates)
        {
            var ret = new List<DateTime>();

            var avDates = availableDates.Select(s => s.Date).ToList();

            for (var i = min; i < max; i = i.AddDays(1))
            {
                if (!avDates.Contains(i))
                {
                    ret.Add(i);
                }
            }

            return ret;
        }
    }
}