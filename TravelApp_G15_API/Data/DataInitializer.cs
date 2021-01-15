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

        public async System.Threading.Tasks.Task InitalizeData()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {
                #region Collections 
                ICollection<Trip> tripsA = new List<Trip>();
                ICollection<Category> categoriesA = new List<Category>();
                ICollection<Location> locationsFrankrijk = new List<Location>();
                ICollection<Location> locationsDuitsland = new List<Location>();
                ICollection<Item> itemsA = new List<Item>();
                #endregion

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

                #region Location
                var locationParijs= new Location() {  City = "Paris", Country = "Frankrijk" };
                var locationNice= new Location() { City = "Nice", Country = "Frankrijk" };
                var locationBerlijn = new Location() { City = "Berlijn", Country = "Duitsland" };
                var locationHamburg = new Location() { City = "Hamburg", Country = "Duitsland" };

                locationsFrankrijk.Add(locationParijs);
                locationsFrankrijk.Add(locationNice);
                locationsDuitsland.Add(locationBerlijn);
                locationsDuitsland.Add(locationHamburg);

                #endregion

                #region Categorie 
                var categorieBadgrief = new Category() { Name = "BadGrief" };
                var categorieElektronica = new Category() { Name = "Elektronica" };
                categoriesA.Add(categorieBadgrief);
                categoriesA.Add(categorieElektronica);
                #endregion

                #region Item
                var itemTandenborstel = new Item() { Name = "Tandenborstel", Amount = 1, Checked = false, Category = categorieBadgrief };
                var itemKam = new Item() { Name = "Kam", Amount = 1, Checked = false, Category = categorieBadgrief };
                var itemHanddoek= new Item() { Name = "Handdoek", Amount = 5, Checked = false, Category = categorieBadgrief };
                var itemNintendo= new Item() { Name = "Nintendo", Amount = 1, Checked = false, Category = categorieElektronica };
                var itemGsm= new Item() { Name = "Gsm", Amount = 1, Checked = false, Category = categorieElektronica };
                itemsA.Add(itemTandenborstel);
                itemsA.Add(itemKam);
                itemsA.Add(itemHanddoek);
                itemsA.Add(itemNintendo);
                itemsA.Add(itemGsm);
                #endregion

                #region Trip 
                var tripFrankrijk = new Trip() { Name = "Frankrijk - chillreisje", Date = new DateTime(2021, 07, 22), Locations = locationsFrankrijk, Categories = categoriesA, Items = itemsA };
                var tripDuitsland = new Trip() { Name = "Duitsland - Hoofdstad bezoeken", Date = new DateTime(2021, 07, 22), Locations = locationsDuitsland, Categories = categoriesA, Items = itemsA };
                tripsA.Add(tripFrankrijk);
                tripsA.Add(tripDuitsland);

                customerOne.Trips.Add(tripFrankrijk);
                customerOne.Trips.Add(tripDuitsland);
                #endregion

                
                _context.Users.Add(customerOne);
                _context.Users.Add(adminOne);
                _context.Trips.AddRange(tripsA);
                _context.Categories.AddRange(categoriesA);
                _context.SaveChanges();
                #endregion
            }
        }
    }
}
