using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Book.API.Dto
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; } =null!;
        [Required]
        public string UserName { get; set; }=null!;
        [Required]
        public string Email { get; set; }=null!;
        [Required]
        public string Password { get; set; }=null!;

    }
}