using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Data;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
            _users = _context.Users;
        }
        public List<User> GetAll()
        {
            return _users
                .Include(t => t.Trips).ThenInclude(c => c.Categories)
                .Include(t => t.Trips).ThenInclude(i => i.Items)
                .Include(t => t.Trips).ThenInclude(l => l.Locations)
                .ToList();
        }

        public User GetByEmail(string email)
        {
            return _users
                .Include(t => t.Trips).ThenInclude(c => c.Categories)
                .Include(t => t.Trips).ThenInclude(i => i.Items)
                .Include(t => t.Trips).ThenInclude(l => l.Locations)
                .FirstOrDefault(u => u.Email == email);
        }
        public bool TryGetTrips(int id, out List<Trip> trips)
        {
            var users = _users.Include(l => l.Trips).FirstOrDefault(a => a.UserID == id);
            trips = users.Trips.ToList();

            return users != null;
        }

        public bool TryGetTrip(int id, int tripID, out Trip trip)
        {
            var users = _users.Include(l => l.Trips).FirstOrDefault(a => a.UserID == id);
            trip = users.Trips.FirstOrDefault(l => l.TripID == tripID);

            return trip != null;
        }

        public void Add(User u)
        {
            _users.Add(u);
        }

        public void Remove(User u)
        {
            _users.Remove(u);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
