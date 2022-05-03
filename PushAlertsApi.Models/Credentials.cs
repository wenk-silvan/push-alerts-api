namespace PushAlertsApi.Models
{
    /// <summary>
    /// Represents the login details of a user
    /// </summary>
    public class Credentials
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}