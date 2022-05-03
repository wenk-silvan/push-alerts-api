using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Data;
using PushAlertsApi.Filters;
using PushAlertsApi.Models;
using PushAlertsApi.Services;

namespace PushAlertsApi.Controllers
{
    /// <summary>
    /// This Api Controller is responsible for all authentication related requests.
    /// It is unauthorized and the endpoints can be reached without authentication.
    /// </summary>
    [ApiController]
    [Route("api/")]
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

        /// <summary>
        /// Calls UserService instance to add the user to the database with a new password hash.
        /// The email and password are getting validated minimal.
        /// This endpoint requires the ApiKey authentication as specified in the custom ApiKeyAuth attribute.
        /// </summary>
        /// <param name="key">The api key as header parameter</param>
        /// <param name="request">The credentials as body</param>
        /// <returns>The new created user with Email and UUID</returns>
        [ApiKeyAuth]
        [HttpPost("register")]
        public ActionResult Register([FromHeader(Name = "ApiKey")] string key, [FromBody] Credentials request)
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

        /// <summary>
        /// Calls the UserService instance to verify email and password
        /// Then a new JSON Web Token gets created based on the configured expiry time and symmetric key
        /// </summary>
        /// <param name="request">The credentials according to the Credentials class</param>
        /// <returns>The new token to use as session key</returns>
        [HttpPost("login")]
        public ActionResult<Token> Login([FromBody] Credentials? request)
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

            var days = int.TryParse(_configuration.GetSection("Jwt:Days").Value, out var result) ? result : 14;
            var expiry = DateTime.Now.AddDays(days).ToUniversalTime();
            var token = _usersService.CreateToken(potentialUser, _configuration.GetSection("Jwt:Key").Value, expiry);
            return Ok(new Token(token, expiry, potentialUser.Email, potentialUser.Uuid));
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