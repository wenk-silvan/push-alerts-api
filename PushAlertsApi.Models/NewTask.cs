namespace PushAlertsApi.Models
{
    public class NewTask
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;
    }
}