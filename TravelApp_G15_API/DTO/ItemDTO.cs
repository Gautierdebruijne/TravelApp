using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp_G15_API.Models;

namespace TravelApp_G15_API.DTO
{
    public class ItemDTO
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool Checked { get; set; }

       public CategoryDTO Category { get; set; }

       public ItemDTO()
        {
            Checked = false;
        }
    }
}
