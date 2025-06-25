using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Book.API.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime DateAdded { get; set; }
    }
}