namespace PushAlertsApi.Util
{
    /// <summary>
    /// This class provides helper methods to increase the quality of application logging.
    /// </summary>
    public static class MessageFormatter
    {
        /// <summary>
        /// Formats a message of an exception, this should be used always when logging an error e.g. an exception occurs.
        /// </summary>
        /// <param name="ex">The occurred exception</param>
        /// <returns>The formatted message</returns>
        public static string LogError(Exception ex)
        {
            return $"Exception Message: {ex.Message}, Inner Exception Message: " +
                   $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}";
        }

        /// <summary>
        /// Use this default message to return in the HTTP response for an unexpected server error, don't return server details.
        /// </summary>
        /// <returns></returns>
        public static string ActionResultBadRequest()
        {
            return $"Unexpected server error";
        }
    }
}