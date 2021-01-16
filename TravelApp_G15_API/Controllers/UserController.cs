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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult<List<User>> GetUserByEmail()
        {
            List<User> u = _userRepository.GetAll();
            return u;
        }

        [HttpGet("trips")]
        public ActionResult<List<Trip>> GetUserTrips()
        {

            if (!_userRepository.TryGetTrips(getLoggedUser().UserID, out var trips))
                NotFound();



            return trips;
        }

        [HttpGet("{tripID}/trip")]
        public ActionResult<Trip> GetUserTrip(int tripID)
        {
           

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID,out var trip))
                NotFound();



                
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

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetLocations(trip.TripID, out var locations))
                NotFound();


            return locations;
        }

        [HttpGet("{tripID}/locations/{locationID}")]
        public ActionResult<Location> GetUserTripLocation(int tripID, int locationID)
        {
          
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetLocation(trip.TripID, locationID, out var location))
                NotFound();

            return location;
        }

        [HttpGet("{tripID}/categories")]
        public ActionResult<List<Category>> GetUserTripCategories(int tripID)
        {
 

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetCategories(trip.TripID, out var categories))
                NotFound();


            return categories;
        }

        [HttpGet("{tripID}/categories/{categoryID}")]
        public ActionResult<Category> GetUserTripCategory( int tripID, int categoryID)
        {

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetCategory(trip.TripID, categoryID, out var category))
                NotFound();

            return category;
        }

        [HttpGet("{tripID}/items")]
        public ActionResult<List<Item>> GetUserTripItems( int tripID)
        {

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetItems(trip.TripID, out var items))
                NotFound();


            return items;
        }

        [HttpGet("{tripID}/items/{itemID}")]
        public ActionResult<Item> GetUserTripItem(int tripID, int itemID)
        {

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetItem(trip.TripID, itemID, out var item))
                NotFound();




            return item;
        }

        [HttpGet("{tripID}/task/{taskID}")]
        public ActionResult<Models.Task> GetUserTripTask(int tripID, int taskID)
        {

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetTask(trip.TripID, taskID, out var task))
                NotFound();




            return task;
        }

        [HttpGet("{tripID}/tasks")]
        public ActionResult<List<Models.Task>> GetUserTripTasks(int tripID)
        {
            //List<ItemDTO> itemList = new List<ItemDTO>();

            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                NotFound();
            if (!_tripRepository.TryGetTasks(trip.TripID, out var tasks))
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

            return tasks;
        }


        #endregion

        #region HttpPostLoginRegister
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

/*        [AllowAnonymous]
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
*/
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<ActionResult<string>>> RegisterAsync(RegisterDTO model)
        {
            IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email };
            User u = new User{ Name = model.Name, Email = model.Email };

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

        #region HttpPostTrip
        [HttpPost("addTrip")]
        public IActionResult AddTrip(TripDTO t)
        {
            Trip trip = new Trip() { Name = t.Name, Date = t.Date };
            getLoggedUser().AddTrip(trip);
            _userRepository.SaveChanges();
            return NoContent();
        }

        [HttpPost("{tripID}/addCategorie")]
        public IActionResult AddCategorie(int tripID, CategoryDTO categoryDTO)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NotFound();
            var categorie = new Category() { Name = categoryDTO.Name };
            trip.AddCategory(categorie);
            _tripRepository.SaveChanges();
            return NoContent();
        }

        [HttpPost("{tripID}/addItem")]
        public IActionResult AddItem(int tripID, ItemDTO itemDTO)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NotFound();
            var item = new Item() { Name = itemDTO.Name, Amount = itemDTO.Amount, Category = null,  Checked = false};
            trip.AddItem(item);
            _tripRepository.SaveChanges();
            return NoContent();
        }

        [HttpPost("{tripID}/addLocation")]
        public IActionResult AddLocation(int tripID, LocationDTO locationDTO)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NotFound();
            var location = new Location() { Country = locationDTO.Country, City = locationDTO.City };
            trip.AddLocation(location);
            _tripRepository.SaveChanges();
            return NoContent();
        }

        [HttpPost("{tripID}/addTask")]
        public IActionResult AddTask(int tripID, TaskDTO taskDTO)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NotFound();
            var task = new Models.Task() { Name = taskDTO.Name, isChecked = taskDTO.isChecked };
            trip.AddTask(task);
            _tripRepository.SaveChanges();
            return NoContent();
        }




        [HttpPost("{tripID}/{categorieID}/Categorie/{itemID}/addItemToCategory")]
        public IActionResult AddItemToCategorie(int tripID, int categorieID, int itemID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NotFound();
            if (!_tripRepository.TryGetCategory(trip.TripID, categorieID, out var category))
                return NotFound();
            if (!_tripRepository.TryGetItem(trip.TripID, itemID, out var item))
                return NotFound();

            item.Category = category; 
            _tripRepository.SaveChanges();
            return NoContent();
        }

        #endregion
        #region Delete

        [HttpDelete("{tripID}/Items/{itemID}")]
        public IActionResult DeleteItem(int tripID, int itemID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NoContent();
            if (!_tripRepository.TryGetItem(trip.TripID, itemID, out var item))
                return NoContent();

              trip.RemoveItem(item);
            _tripRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{tripID}/Category/{categoryID}")]
        public IActionResult DeleteCategory(int tripID, int categoryID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NoContent();
            if (!_tripRepository.TryGetCategory(trip.TripID, categoryID, out var category))
                return NoContent();

            trip.RemoveCategory(category);
            _tripRepository.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{tripID}/Location/{locationID}")]
        public IActionResult DeleteLocation(int tripID, int locationID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NoContent();
            if (!_tripRepository.TryGetLocation(trip.TripID, locationID, out var location))
                return NoContent();

            trip.RemoveLocation(location);
            _tripRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{tripID}/Task/{taskID}")]
        public IActionResult DeleteTask(int tripID, int taskID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NoContent();
            if (!_tripRepository.TryGetTask(trip.TripID, taskID, out var task))
                return NoContent();

            trip.RemoveTask(task);
            _tripRepository.SaveChanges();

            return NoContent();
        }



        #endregion

        #region put 
        [HttpPut("{tripID}/Item/{itemID}")]
        public IActionResult CheckItem(int tripID, int itemID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NoContent();

            if (!_tripRepository.TryGetItem(trip.TripID, itemID, out var item))
                return NoContent();

            item.Checked = !item.Checked;
            _tripRepository.UpdateTrip(trip);
            _tripRepository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{tripID}/Task/{taskID}")]
        public IActionResult CheckTask(int tripID, int taskID)
        {
            if (!_userRepository.TryGetTrip(getLoggedUser().UserID, tripID, out var trip))
                return NoContent();
            if (!_tripRepository.TryGetTask(trip.TripID, taskID, out var task))
                return NoContent();

            task.isChecked = !task.isChecked;
            _tripRepository.UpdateTrip(trip);
            _tripRepository.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
