﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp_G15.Models
{
    class Register
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public Register(string name, string email, string password, string passwordconfirm)
        {
            Name = name;
            Email = email;
            Password = password;
            PasswordConfirm = passwordconfirm;
        }
    }
}
