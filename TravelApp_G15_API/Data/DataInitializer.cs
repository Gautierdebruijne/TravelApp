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
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(DatabaseContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitalizeData()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {
                #region Roles
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                #endregion

                #region Users
                var customerOne = new User() { Name = "Customer", Email = "customer@example.com" };
                var iCustomerOne = new IdentityUser() { UserName = customerOne.Email, Email = customerOne.Email };

                var adminOne = new User() { Name = "Admin", Email = "admin@example.com" };
                var iAdminOne = new IdentityUser() { UserName = adminOne.Email, Email = adminOne.Email };
 
                await _userManager.CreateAsync(iCustomerOne, "Customer8!");
                await _userManager.AddToRoleAsync(iCustomerOne, "Customer");

                await _userManager.CreateAsync(iAdminOne, "AdminOne8!");
                await _userManager.AddToRoleAsync(iAdminOne, "Admin");

                _context.Users.Add(customerOne);
                _context.Users.Add(adminOne);
                _context.SaveChanges();
                #endregion
            }
        }
    }
}
