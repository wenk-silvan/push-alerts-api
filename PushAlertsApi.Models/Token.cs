namespace PushAlertsApi.Models
{
    public class Token
    {
        public string Value { get; set; }
        public DateTime ExpiryUtc { get; set; }

        public Token(string value, DateTime expiryUtc)
        {
            Value = value;
            ExpiryUtc = expiryUtc;
        }
    }
}