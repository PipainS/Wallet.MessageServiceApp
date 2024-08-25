namespace MessageService.API.Models.DTOs
{
    public class MessageDto
    {
        public string? UserName { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int ClientOrder { get; set; }
    }
}
