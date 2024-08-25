namespace MessageService.API.Models
{
    /// <summary>
    /// Represents a message entity with details about the message content and metadata.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the name of the user who sent the message. This property is optional.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the message.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the message was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who sent the message.
        /// </summary>
        public int UserId { get; set; }
    }
}
