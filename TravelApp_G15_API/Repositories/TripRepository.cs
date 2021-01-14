﻿using Microsoft.EntityFrameworkCore;
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
            return _trips
                .Include(l => l.Locations)
                .Include(c => c.Categories)
                .Include(i => i.Items)
                .ToList();
        }

        public Trip GetById(int id)
        {
            return _trips
                .Include(c => c.Categories)
                .Include(i => i.Items)
                .FirstOrDefault(t => t.TripID == id);
        }


        public bool TryGetLocations(int id, out List<Location> locations)
        {
            var trip = _trips.Include(l => l.Locations).FirstOrDefault(a => a.TripID == id);
            locations = trip.Locations.ToList();

            return locations != null;
        }

        public bool TryGetLocation(int id, int locID, out Location location)
        {
            var trip = _trips.Include(l => l.Locations).FirstOrDefault(a => a.TripID == id);
            location = trip.Locations.FirstOrDefault(l => l.LocationID == locID);

            return location != null;
        }

        public bool TryGetCategories(int id, out List<Category> categories)
        {
            var trip = _trips.Include(l => l.Categories).FirstOrDefault(a => a.TripID == id);
            categories = trip.Categories.ToList();

            return categories != null;
        }

        public bool TryGetCategory(int id, int catID, out Category category)
        {
            var trip = _trips.Include(l => l.Categories).FirstOrDefault(a => a.TripID == id);
            category = trip.Categories.FirstOrDefault(l => l.CategoryID == catID);

            return category != null;
        }

        public bool TryGetItems(int id, out List<Item> items)
        {
            var trip = _trips.Include(l => l.Items).FirstOrDefault(a => a.TripID == id);
            items = trip.Items.ToList();

            return items != null;
        }

        public bool TryGetItem(int id, int itemID, out Item item)
        {
            var trip = _trips.Include(l => l.Items).FirstOrDefault(a => a.TripID == id);
            item = trip.Items.FirstOrDefault(l => l.ItemID == itemID);

            return item != null;
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
