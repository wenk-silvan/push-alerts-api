using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PushAlertsApi.Models;

namespace PushAlertsApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;

        private readonly DbSet<User> _dbSet;

        public UsersService(DbSet<User> context)
        {
            _logger = new LoggerFactory().CreateLogger<UsersService>();
            _dbSet = context;
        }

        public User GetUserByUuid(string uuid)
        {
            var user = _dbSet.Single(u => u.Uuid == Guid.Parse(uuid));
            _logger.LogInformation($"Fetched user by uuid from database with id: '{user.Id}'.");
            return user;
        }

        public User? GetUserByEmail(string email)
        {
            var user = _dbSet.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                _logger.LogInformation($"Fetched user by email from database with id: '{user.Id}'.");
            }

            return user;
        }

        public User AddUser(User user)
        {
            var entity = _dbSet.Add(user).Entity;
            _logger.LogInformation($"Added user to database with id: '{user.Id}'.");
            return entity;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public string CreateToken(User user, string jwtKey, DateTime expiry)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}