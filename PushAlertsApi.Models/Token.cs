namespace PushAlertsApi.Models
{
    /// <summary>
    /// Represents the token which is returned after a successful user login. The Value property is the actual JSON Web Token.
    /// </summary>
    public class Token
    {
        public string Value { get; set; }
        public DateTime ExpiryUtc { get; set; }

        public string Email { get; set; }

        public Guid Uuid { get; set; }

        public Token(string value, DateTime expiryUtc, string email, Guid uuid)
        {
            Value = value;
            ExpiryUtc = expiryUtc;
            Email = email;
            Uuid = uuid;
        }
    }
}