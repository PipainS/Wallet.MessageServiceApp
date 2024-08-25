namespace MessageService.API.Models
{
    public class Message
    {
        public string? UserName { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int ClientOrder { get; set; }
    }
}
