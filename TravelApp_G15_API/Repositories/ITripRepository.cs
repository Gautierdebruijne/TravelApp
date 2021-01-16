using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public interface ITripRepository
    {
        List<Trip> GetAll();
        Trip GetById(int id);


        bool TryGetCategory(int tripID, int catID, out Category category);
        bool TryGetCategories(int tripID, out List<Category> categories);
        bool TryGetLocation(int tripID, int locationID, out Location location);
        bool TryGetLocations(int tripID, out List<Location> locations);
        bool TryGetItem(int tripID, int itemID, out Item item);
        bool TryGetItems(int tripID, out List<Item> items);
        bool TryGetTask(int tripID, int taskID, out Models.Task task);
        bool TryGetTasks(int tripID, out List<Models.Task> tasks);
        bool TryGetItemsFromCategory(int id, int categoryID, out List<Item> items);


        void AddTrip(Trip t);
        void DeleteTrip(Trip t);
        void UpdateTrip(Trip t);
        void SaveChanges();
    }
}
