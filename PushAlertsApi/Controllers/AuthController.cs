using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Register([FromBody] Credentials request)
        {
            var email = request?.Email.Trim().ToLower();
            var password = request?.Password.Trim();
            if (!IsEmailValid(email) || !IsPasswordValid(password))
            {
                return BadRequest("Invalid email or password.");
            }

            if (_usersService.GetUserByEmail(email!) != null)
            {
                return BadRequest("User already exists.");
            }

            _usersService.CreatePasswordHash(password!, out var passwordHash, out var passwordSalt);
            var user = _usersService.AddUser(new User(email!, passwordHash, passwordSalt));
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] Credentials? request)
        {
            var email = request?.Email.Trim().ToLower();
            var password = request?.Password.Trim();

            var potentialUser = _usersService.GetUserByEmail(email!);
            if (potentialUser == null || potentialUser.Email != email)
            {
                return BadRequest("User not found.");
            }

            if (!_usersService.VerifyPasswordHash(password!, potentialUser.PasswordHash,
                    potentialUser.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            var token = _usersService.CreateToken(potentialUser, _configuration.GetSection("Jwt:Key").Value);
            return Ok(token);
        }

        private static bool IsEmailValid(string? email)
        {
            return !email.IsNullOrEmpty()
                   && email!.Contains("@")
                   && email.Contains(".");
        }

        private static bool IsPasswordValid(string? password)
        {
            return !password.IsNullOrEmpty();
        }
    }
}