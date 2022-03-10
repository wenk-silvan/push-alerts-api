namespace PushAlertsApi.Models.Dto
{
    public class UserDto
    {
        public Guid Uuid { get; set; }
        public string Email { get; set; }
        public ICollection<Guid> ProjectUuids { get; set; }
    }
}