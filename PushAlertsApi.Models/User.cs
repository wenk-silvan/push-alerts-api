using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Models
{
    public class User
    {
        public int Id { get; set;  }
        public Guid Uuid { get; set;  }
        public string Email { get; set; }
        public List<Project>? Projects { get; set; }

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

        public User(UserDto dto)
        {
            Uuid = dto.Uuid;
            Email = dto.Email;
        }
    }
}