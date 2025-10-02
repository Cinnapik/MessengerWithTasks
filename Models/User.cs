// C:\Users\St\MessengerWithTasks\MessengerApp\Models\User.cs
using System;

namespace MessengerApp.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarColor { get; set; } = "#FF3B82F6"; // default accent
        public string Password { get; set; } = string.Empty; // plain for prototype only
    }
}
