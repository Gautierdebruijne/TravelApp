using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Data;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Category> _categories;

        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
            _categories = _context.Categories;
        }

        public List<Category> GetAll()
        {
            return _categories.ToList();
        }

        public Category GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.CategoryID == id);
        }
        public void AddCategory(Category category)
        {
            _categories.Add(category);
        }

        public void DeleteCategory(Category category)
        {
            _categories.Remove(category);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
