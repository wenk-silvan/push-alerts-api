namespace PushAlertsApi.Models
{
    public class User
    {
        public User(Guid uuid, string email)
        {
            Uuid = uuid;
            Email = email;
        }

        public Guid Uuid { get; set; }
        public string Email { get; set; }
    }
}