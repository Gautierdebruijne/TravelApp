using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        List<User> GetAll();
        bool TryGetUserIDbyEmail(String email, out int userID);
        bool TryGetTrips(int userID, out List<Trip> trips);
        bool TryGetTrip(int userID, int tripID, out Trip trip);
        void Add(User u);
        void Remove(User u);
        void SaveChanges();
    }
}
