using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Data
{
    public class DataInitializer
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DataInitializer(DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task InitalizeData()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {
                #region Users
                var userOne = new User() { Name = "Customer", Email = "customer@example.com" };
                var identityOne = new IdentityUser() { UserName = userOne.Email, Email = userOne.Email };

                await _userManager.CreateAsync(identityOne, "Customer8!");

                _context.Users.Add(userOne);
                _context.SaveChanges();
                #endregion
            }
        }
    }
}
