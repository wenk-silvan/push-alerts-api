using System.Text.Json.Serialization;

namespace PushAlertsApi.Models
{
    public class User
    {
        [JsonIgnore] public int Id { get; set; }

        public Guid Uuid { get; set; }

        public string Email { get; set; }

        [JsonIgnore] public virtual List<Project>? Projects { get; set; } = new();

        public User(string email)
        {
            Uuid = Guid.NewGuid();
            Email = email;
        }

        public User(Guid uuid, string email)
        {
            Uuid = uuid;
            Email = email;
        }
    }
}