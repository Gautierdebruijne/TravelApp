using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public interface ITripRepository
    {
        public List<Trip> GetAll();
        public Trip GetById(int id);
        public void AddTrip(Trip t);
        public void DeleteTrip(Trip t);
        public void SaveChanges();
    }
}
