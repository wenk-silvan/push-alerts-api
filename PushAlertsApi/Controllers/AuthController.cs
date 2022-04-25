using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Services;

namespace PushAlertsApi.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
            _usersService = new UsersService(context.Users);
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] UserDto request)
        {
            _usersService.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
            var user = _usersService.AddUser(new User(request.Email, passwordHash, passwordSalt));
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] UserDto request)
        {
            var user = _usersService.GetUserByEmail(request.Email);
            if (user == null || user.Email != request.Email)
            {
                return BadRequest("User not found.");
            }

            if (!_usersService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            var token = _usersService.CreateToken(user, _configuration.GetSection("Jwt:Key").Value);
            return Ok(token);
        }
    }
}