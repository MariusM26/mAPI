﻿using System.ComponentModel.DataAnnotations.Schema;

namespace mAPI.Database.Models
{
    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
