using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Data;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Trip> _trips;

        public TripRepository(DatabaseContext context)
        {
            _context = context;
            _trips = _context.Trips;
        }

        public List<Trip> GetAll()
        {
            return _trips.ToList();
        }

        public Trip GetById(int id)
        {
            return _trips.FirstOrDefault(t => t.TripID == id);
        }

        public void AddTrip(Trip t)
        {
            _trips.Add(t);
        }

        public void DeleteTrip(Trip t)
        {
            _trips.Remove(t);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
