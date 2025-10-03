using MessengerApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApp.Services
{
    // Очень простое in-memory хранилище с минимальной сериализацией (prototype)
    public class StorageService
    {
        public readonly Dictionary<string, List<Message>> MessagesByChat = new();
        public readonly List<TaskItem> Tasks = new();

        public StorageService()
        {
            // seed some data
            if (!MessagesByChat.ContainsKey("default"))
                MessagesByChat["default"] = new List<Message>
                {
                    new Message{ Id="1", ChatId="default", Sender="Антон", Content="Привет всем", Timestamp=System.DateTime.Now.AddMinutes(-60) },
                    new Message{ Id="2", ChatId="default", Sender="Саша", Content="Привет", Timestamp=System.DateTime.Now.AddMinutes(-50) }
                };
        }
    }
}
