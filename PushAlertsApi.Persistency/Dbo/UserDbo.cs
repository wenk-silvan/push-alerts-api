namespace PushAlertsApi.Persistency.Dto
{
    public class UserDbo
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> ProjectUuids { get; set; }
    }
}