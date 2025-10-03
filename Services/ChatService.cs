using MessengerApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApp.Services
{
    public class ChatService
    {
        private readonly StorageService _storage;

        public ChatService(StorageService storage)
        {
            _storage = storage;
        }

        public IEnumerable<Message> GetMessages(string chatId)
        {
            if (string.IsNullOrEmpty(chatId)) chatId = "default";
            if (!_storage.MessagesByChat.ContainsKey(chatId)) return Enumerable.Empty<Message>();
            return _storage.MessagesByChat[chatId].OrderBy(m => m.Timestamp);
        }

        public void AddMessage(Message m)
        {
            if (m == null) return;
            if (string.IsNullOrEmpty(m.ChatId)) m.ChatId = "default";
            if (!_storage.MessagesByChat.ContainsKey(m.ChatId)) _storage.MessagesByChat[m.ChatId] = new List<Message>();
            _storage.MessagesByChat[m.ChatId].Add(m);
        }
    }
}
