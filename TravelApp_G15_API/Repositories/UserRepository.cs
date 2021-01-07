using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Data;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
            _users = _context.Users;
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public void Add(User u)
        {
            _users.Add(u);
        }

        public void Remove(User u)
        {
            _users.Remove(u);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
