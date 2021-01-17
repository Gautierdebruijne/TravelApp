using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetById(int id);
        public void AddCategory(Category category);
        public void DeleteCategory(Category category);
        public void SaveChanges();
    }
}
