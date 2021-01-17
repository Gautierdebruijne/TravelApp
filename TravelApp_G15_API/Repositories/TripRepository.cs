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
            return _trips
                .Include(l => l.Locations)
                .Include(c => c.Categories)
                .Include(i => i.Items)
                .Include(t => t.Tasks)
                .ToList();
        }

        public Trip GetById(int id)
        {
            return _trips
            .Include(l => l.Locations)
                .Include(c => c.Categories)
                .Include(i => i.Items)
                .Include(t => t.Tasks)
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

        public bool TryGetTasks(int id, out List<Models.Task> tasks)
        {
            var trip = _trips.Include(l => l.Tasks).FirstOrDefault(a => a.TripID == id);
            tasks = trip.Tasks.ToList();

            return tasks != null;
        }

        public bool TryGetItemsFromCategory(int id, int categoryID, out List<Item> items)
        {
            var trip = _trips.Include(l => l.Items).FirstOrDefault(t => t.TripID == id);
            items = trip.Items.Where(i => i.Category.CategoryID == categoryID).ToList();

            return items != null;
        }



        public bool TryGetTask(int id, int taskID, out Models.Task task)
        {
            var trip = _trips.Include(l => l.Tasks).FirstOrDefault(a => a.TripID == id);
            task = trip.Tasks.FirstOrDefault(l => l.TaskID == taskID);

            return task != null;
        }

        public void AddTrip(Trip t)
        {
            _trips.Add(t);
        }

        public void DeleteTrip(Trip t)
        {
            _trips.Remove(t);
        }

        public void UpdateTrip(Trip t)
        {
            _trips.Update(t);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void CheckTask(int id, int taskID, Models.Task task)
        {
            var trip = _trips.Include(l => l.Tasks).FirstOrDefault(a => a.TripID == id);

            Models.Task tasktodo = trip.Tasks.FirstOrDefault(t => t.TaskID == taskID);

            tasktodo.Name = task.Name;
            tasktodo.isChecked = task.isChecked;
        }

        public void CheckItem(int id, int itemID, Item item)
        {
            var trip = _trips.Include(l => l.Items).FirstOrDefault(a => a.TripID == id);

            Item done = trip.Items.FirstOrDefault(t => t.ItemID == itemID);

            done.Name = item.Name;
            done.Checked = item.Checked;
        }
    }
}
