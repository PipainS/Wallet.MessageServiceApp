namespace MessageService.API.Constants
{
    /// <summary>
    /// Contains constant values used throughout the messaging application.
    /// </summary>
    public static class MessageConstants
    {
        /// <summary>
        /// The default username used when no username is provided. 
        /// This is set to "Аноним" (Anonymous in Russian).
        /// </summary>
        public const string DefaultUserName = "Аноним";

        /// <summary>
        /// The format string used for displaying timestamps.
        /// This format represents date and time in the format "yyyy-MM-dd HH:mm".
        /// </summary>
        public const string TimestampFormat = "yyyy-MM-dd HH:mm";
    }
}
