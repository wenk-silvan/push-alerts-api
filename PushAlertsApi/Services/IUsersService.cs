using PushAlertsApi.Models;

namespace PushAlertsApi.Services
{
    /// <summary>
    /// Serves as a contract for the UsersService to meet the business needs.
    /// This service should do CRUD operations to the database context regarding users and authentication related work.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Returns the user for the given Uuid
        /// </summary>
        /// <param name="uuid">A unique identifier for a user</param>
        /// <returns></returns>
        public User GetUserByUuid(string uuid);

        /// <summary>
        /// Returns the user for the given email
        /// </summary>
        /// <param name="email">An email address</param>
        /// <returns></returns>
        public User? GetUserByEmail(string email);

        /// <summary>
        /// Adds user as new entity to the database context
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User AddUser(User user);

        /// <summary>
        /// Computes a SHA512 hash from the given password and writes hash and salt (generated) to out parameters
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        /// <summary>
        /// Creates a new jwt token for symmetric encryption from given jwt key and using the claims (email) of the user
        /// </summary>
        /// <param name="user">The user to get the email as user claim</param>
        /// <param name="jwtKey">The encryption key</param>
        /// <param name="expiry">Any DateTime in the future</param>
        /// <returns></returns>
        public string CreateToken(User user, string jwtKey, DateTime expiry);

        /// <summary>
        /// Computes the given password and verifies equality to given salted hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}