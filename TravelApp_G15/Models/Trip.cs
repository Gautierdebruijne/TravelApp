﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp_G15.Models
{
    public class Trip
    {
        public int TripID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Item> Items { get; set; }

        public Trip()
        {
            Date = DateTime.Now.Date;

            Locations = new List<Location>();
            Categories = new List<Category>();
            Items = new List<Item>();
        }

        public Trip(DateTime date) : this()
        {
            Date = date;
        }
    }
}
