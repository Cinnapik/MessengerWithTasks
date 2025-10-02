// C:\Users\St\MessengerWithTasks\MessengerApp\Models\Message.cs
using System;

namespace MessengerApp.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string ChatId { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsSentByMe { get; set; }
    }
}
