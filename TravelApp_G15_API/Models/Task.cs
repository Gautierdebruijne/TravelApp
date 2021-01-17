using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp_G15_API.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string Name { get; set; }
        public bool isChecked { get; set; }
    }
}
