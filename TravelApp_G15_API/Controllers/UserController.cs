using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp_G15_API.Models;
using TravelApp_G15_API.Repositories;

namespace TravelApp_G15_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        
        public UserController(IUserRepository repo)
        {
            _userRepository = repo;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUserByEmail()
        {
            List<User> u = _userRepository.GetAll();
            return u;
        }
    }
}
