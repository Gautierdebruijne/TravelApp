using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp_G15_API.Models
{
    public class Trip
    {
        public int TripID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Task> Tasks { get; set; }



        public Trip()
        {
            Date = DateTime.Now.Date;

            Locations = new List<Location>();
            Categories = new List<Category>();
            Items = new List<Item>();
            Tasks = new List<Task>();
        }

        public Trip(DateTime date) : this()
        {
            Date = date;
        }

        public void AddLocation(Location location) => Locations.Add(location);
        public void AddCategory(Category category) => Categories.Add(category);
        public void AddItem(Item item) => Items.Add(item);
        public void AddTask(Task task) => Tasks.Add(task);



        public void RemoveLocation(Location location) => Locations.Remove(location);
        public void RemoveCategory(Category category) => Categories.Remove(category);
        public void RemoveItem(Item item) => Items.Remove(item);
        public void RemoveTask(Task task) => Tasks.Remove(task);

    }
}
