using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp_G15_API.DTO;
using TravelApp_G15_API.Models;
using TravelApp_G15_API.Repositories;

namespace TravelApp_G15_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;

        public TripController(ITripRepository tripRepo, IUserRepository userRepo)
        {
            _tripRepository = tripRepo;
            _userRepository = userRepo;
        }

        
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<Trip>> GetAll()
        {
            List<Trip> t = _tripRepository.GetAll();
            return t;
        }



        [HttpGet("tripID")]
        public ActionResult<Trip> GetById(int tripID)
        {
            Trip t = _tripRepository.GetById(tripID);
            return t;
        }

        [HttpPost]
        public ActionResult<Trip> AddTrip(TripDTO dto)
        {
            Trip t = new Trip() { Name = dto.Name, Date = dto.Date };

            _tripRepository.AddTrip(t);
            _tripRepository.SaveChanges();

            return t;
        }

        [HttpDelete("tripID")]
        public ActionResult<Trip> RemoveTrip(int tripID)
        {
            Trip t = _tripRepository.GetById(tripID);

            _tripRepository.DeleteTrip(t);
            _tripRepository.SaveChanges();

            return t;
        }
    }
}
