using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public interface IUserRepository
    {
        public User GetByEmail(string email);
        public List<User> GetAll();
        public void Add(User u);
        public void Remove(User u);
        public void SaveChanges();
    }
}
