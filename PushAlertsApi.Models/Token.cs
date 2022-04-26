namespace PushAlertsApi.Models
{
    public class Token
    {
        public string Value { get; set; }
        public DateTime ExpiryUtc { get; set; }

        public string Email { get; set; }

        public Guid UUID { get; set; }

        public Token(string value, DateTime expiryUtc, string email, Guid uuid)
        {
            Value = value;
            ExpiryUtc = expiryUtc;
            Email = email;
            UUID = UUID;
        }
    }
}