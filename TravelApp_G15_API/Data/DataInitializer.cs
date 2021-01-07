using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp_G15_API.Data
{
    public class DataInitializer
    {
        private readonly DatabaseContext _context;

        public DataInitializer(DatabaseContext context)
        {
            _context = context;
        }

        public async Task InitalizeData()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {

            }
        }
    }
}
