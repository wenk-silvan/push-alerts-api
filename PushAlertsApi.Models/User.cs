using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Email { get; set; }

        public static User Copy(UserDto dto)
        {
            return new User
            {
                Uuid = dto.Uuid,
                Email = dto.Email,
            };
        }
    }
}