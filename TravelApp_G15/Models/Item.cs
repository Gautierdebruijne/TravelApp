using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp_G15.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool Checked { get; set; }
        public Category Category { get; set; }
    }
}
