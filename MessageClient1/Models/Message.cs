namespace MessageClient1.Models
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserName { get; set; }
        public int UserId { get; set; }

    }
}
