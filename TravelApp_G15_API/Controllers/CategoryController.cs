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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository repo)
        {
            _categoryRepository = repo;
        }

        #region HttpGet
        [HttpGet]
        //getbyTripID
        public ActionResult<List<Category>> GetAll()
        {
            List<Category> c = _categoryRepository.GetAll();
            return c;
        }

        [HttpGet("categoryID")]
        public ActionResult<Category> GetById(int categoryID)
        {
            Category c = _categoryRepository.GetById(categoryID);
            return c;
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult<Category> AddCategory(CategoryDTO dto)
        {
            Category c = new Category { Name = dto.Name };

            _categoryRepository.AddCategory(c);
            _categoryRepository.SaveChanges();

            return c;
        }
        #endregion

        #region HttpDelete
        [HttpDelete("categoryID")]
        public ActionResult<Category> RemoveCategory(int categoryID)
        {
            Category c = _categoryRepository.GetById(categoryID);

            _categoryRepository.DeleteCategory(c);
            _categoryRepository.SaveChanges();

            return c;
        }
        #endregion
    }
}
