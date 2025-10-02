// C:\Users\St\MessengerWithTasks\MessengerApp\Services\ChatService.cs
// Сервис для чтения и записи сообщений
using MessengerApp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace MessengerApp.Services
{
    public class ChatService
    {
        private readonly StorageService _storage;
        public ChatService(StorageService storage) => _storage = storage;

        public IEnumerable<Message> GetMessages(string chatId)
        {
            var list = new List<Message>();
            using var conn = _storage.GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, ChatId, Content, Timestamp, IsSentByMe FROM Messages WHERE ChatId = $chatId ORDER BY Timestamp";
            cmd.Parameters.AddWithValue("$chatId", chatId ?? string.Empty);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Message
                {
                    Id = rdr.GetInt64(0),
                    ChatId = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Content = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    Timestamp = DateTime.Parse(rdr.IsDBNull(3) ? DateTime.Now.ToString("o") : rdr.GetString(3)),
                    IsSentByMe = rdr.GetInt32(4) == 1
                });
            }
            return list;
        }

        public void AddMessage(Message m)
        {
            using var conn = _storage.GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Messages (ChatId, Content, Timestamp, IsSentByMe) VALUES ($chatId, $content, $ts, $isSent)";
            cmd.Parameters.AddWithValue("$chatId", m.ChatId ?? string.Empty);
            cmd.Parameters.AddWithValue("$content", m.Content ?? string.Empty);
            cmd.Parameters.AddWithValue("$ts", m.Timestamp.ToString("o"));
            cmd.Parameters.AddWithValue("$isSent", m.IsSentByMe ? 1 : 0);
            cmd.ExecuteNonQuery();
        }
    }
}
