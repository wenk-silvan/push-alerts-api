namespace PushAlertsApi.Util
{
    public static class MessageFormatter
    {
        public static string LogError(Exception ex)
        {
            return $"Exception Message: {ex.Message}, Inner Exception Message: " +
                   $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}";
        }

        public static string ActionResultBadRequest()
        {
            return $"Unexpected server error";
        }
    }
}