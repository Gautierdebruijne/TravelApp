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

        public Trip()
        {
            Date = DateTime.Now.Date;
            Locations = new List<Location>();
            Categories = new List<Category>();
        }

        public Trip(DateTime date) : this()
        {
            Date = date;
        }
    }
}
