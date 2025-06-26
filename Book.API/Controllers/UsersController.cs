using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Book.API.Dto;
using Book.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace Book.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger; 

        public UsersController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IConfiguration configuration,ILogger<UsersController> logger) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(RegisterDto model)
        {
            
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Kullanıcı kayıt isteği geçersiz model ile yapıldı.");
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                DateAdded = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                _logger.LogInformation("Yeni kullanıcı oluşturuldu: {UserName}", user.UserName);
                return StatusCode(201);
            }

            _logger.LogWarning("Kullanıcı oluşturulamadı: {@Errors}", result.Errors);
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            _logger.LogInformation("Giriş denemesi: {Email}", model.Email);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                _logger.LogWarning("Geçersiz e-posta ile giriş denemesi: {Email}", model.Email);
                return BadRequest(new {message = "email hatalı"});
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if(result.Succeeded)
            {
                _logger.LogInformation("Kullanıcı girişi başarılı: {UserName}", user.UserName);
                return Ok(new {token = GenerateJWT(user)});
            }

            _logger.LogWarning("Kullanıcı girişi başarısız: {UserName}", user.UserName);
            return Unauthorized();
        }

        private object GenerateJWT(AppUser user)
        {
            _logger.LogInformation("JWT token oluşturuluyor: {UserId}", user.Id);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("appsettings:Secret").Value ?? "");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName ?? "")
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "sabanulukaya.com"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    
    }
}