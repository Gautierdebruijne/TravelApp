﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp_G15_API.DTO
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
