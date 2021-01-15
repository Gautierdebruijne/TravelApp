using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TravelApp_G15_API.DTO;
using TravelApp_G15_API.Models;
using TravelApp_G15_API.Repositories;

namespace TravelApp_G15_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private User u;

        private readonly IUserRepository _userRepository;
        private readonly ITripRepository _tripRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        
        public UserController(IUserRepository repo,ITripRepository tripRepo, SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _userRepository = repo;
            _tripRepository = tripRepo;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        private User getLoggedUser()
        {
            return _userRepository.GetByEmail(User.Identity.Name);
        }

        #region HttpGet
        [HttpGet]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<List<User>> GetUserByEmail()
        {
            List<User> u = _userRepository.GetAll();
            return u;
        }


        [HttpGet("trips")]
        public ActionResult<List<Trip>> GetUserTrips()
        {
            //List<Trip> tripList = new List<Trip>();

            if (!_userRepository.TryGetTrips(getLoggedUser().UserID, out var trips))
                NotFound();

        /*    foreach(var t in trips)
            {
                var dto = new TripDTO
                {
                    Name = t.Name,
                    Date = t.Date
                };

                tripList.Add(dto);
            }*/

            return trips;
        }

        [HttpGet("{tripID}/trip")]
        public ActionResult<Trip> GetUserTrips(int tripID)
        {
           

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID,out var trip))
                NotFound();

      /*          var dto = new TripDTO
                {
                    Name = trip.Name,
                    Date = trip.Date
                };*/

                
            return trip;
        }

        [HttpGet("{email}")]
        public ActionResult<int> GetUserIDbyEmail(String email)
        {
            if (!_userRepository.TryGetUserIDbyEmail(email, out var userID))
                NotFound();

            return userID;
        }

        [HttpGet("{tripID}/locations")]
        public ActionResult<List<Location>> GetUserTripLocations(int tripID)
        {
            //List<LocationDTO> locationList = new List<LocationDTO>();

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetLocations(trip.TripID, out var locations))
                NotFound();

      /*      foreach(var l in locations)
            {
                var dto = new LocationDTO
                {
                    City = l.City,
                    Country = l.Country
                };
                locationList.Add(dto);
            }*/

            return locations;
        }

        [HttpGet("{tripID}/locations/{locationID}")]
        public ActionResult<Location> GetUserTripLocation(int tripID, int locationID)
        {
          
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetLocation(trip.TripID, locationID, out var location))
                NotFound();


         /*   var locationDTO = new LocationDTO
            {
                City = location.City,
                Country = location.Country
            };
               */
            

            return location;
        }

        [HttpGet("{tripID}/categories")]
        public ActionResult<List<Category>> GetUserTripCategories(int tripID)
        {
            //List<CategoryDTO> categorieList = new List<CategoryDTO>();

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetCategories(trip.TripID, out var categories))
                NotFound();
/*
            foreach (var l in categories)
            {
                var dto = new CategoryDTO
                {
                   Name = l.Name
                };
                categorieList.Add(dto);
            }*/

            return categories;
        }

        [HttpGet("{tripID}/categories/{categoryID}")]
        public ActionResult<Category> GetUserTripCategory( int tripID, int categoryID)
        {

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetCategory(trip.TripID, categoryID, out var category))
                NotFound();


        /*    var categoryDTO = new CategoryDTO
            {
              Name = category.Name
            };
*/


            return category;
        }

        [HttpGet("{tripID}/items")]
        public ActionResult<List<Item>> GetUserTripItems( int tripID)
        {
            //List<ItemDTO> itemList = new List<ItemDTO>();

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetItems(trip.TripID, out var items))
                NotFound();
/*
            foreach (var l in items)
            {
                var dto = new ItemDTO
                {
                   Name = l.Name,
                   Amount = l.Amount,
                   Checked = l.Checked
                };
                itemList.Add(dto);
            }*/

            return items;
        }

        [HttpGet("{tripID}/items/{itemID}")]
        public ActionResult<Item> GetUserTripItem(int tripID, int itemID)
        {

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetItem(trip.TripID, itemID, out var item))
                NotFound();


    /*        var itemDTO = new ItemDTO
            {
                Name = item.Name,
                Amount = item.Amount,
                Checked = item.Checked
            };*/



            return item;
        }

        #endregion

        #region HttpPost
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<String>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                var roles = await _userManager.GetRolesAsync(user);

                if (result.Succeeded)
                {
                    string token = GetToken(user, roles);
                    return Created("", token);
                }
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<String>> Register(RegisterDTO model)
        {
            IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email };
            u = new User() { Name = model.Name, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
                result = await _userManager.AddToRoleAsync(user, model.Role);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    _userRepository.Add(u);
                    _userRepository.SaveChanges();

                    string token = GetToken(user, roles);
                    return Created("", token);
                }
            }

            return BadRequest();
        }

        private string GetToken(IdentityUser user, IList<string> roles)
        {
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            });

            if (roles.Count > 0)
            {
                foreach (var role in roles) { claims.AddClaim(new Claim(ClaimTypes.Role, role)); }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(null, null, claims.Claims, expires: DateTime.Now.AddHours(1), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
