using System;

namespace MessengerApp.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ChatId { get; set; } = "default";
        public string Sender { get; set; } = "Аноним";
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsSentByMe { get; set; } = false;
    }
}
