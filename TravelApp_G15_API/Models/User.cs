using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp_G15_API.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Trip> Trips { get; set; }

        public User() 
        {
            Trips = new List<Trip>();
        }

        public void AddTrip(Trip trip) => Trips.Add(trip);
        public void RemoveTrip(Trip trip) => Trips.Remove(trip);
    }
}
