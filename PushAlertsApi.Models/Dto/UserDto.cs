namespace PushAlertsApi.Models.Dto
{
    public class UserDto
    {
        public Guid Uuid { get; set; }
        public string Email { get; set; }
        public ICollection<Guid> ProjectUuids { get; set; }

        public static UserDto Copy(User dbUser)
        {
            return new UserDto
            {
                Uuid = dbUser.Uuid,
                Email = dbUser.Email,
                ProjectUuids = dbUser.Projects.Select(p => p.Uuid).ToList(),
            };
        }
    }
}