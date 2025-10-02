// C:\Users\St\MessengerWithTasks\MessengerApp\Services\AuthService.cs
using MessengerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApp.Services
{
    // Простая в памяти авторизация для прототипа
    public class AuthService
    {
        private readonly List<User> _users = new();

        public AuthService()
        {
            // предзаполненный тестовый пользователь
            _users.Add(new User
            {
                Id = 1,
                Username = "alice",
                DisplayName = "Alice",
                Password = "password",
                AvatarColor = "#FF6366F1"
            });
            _users.Add(new User
            {
                Id = 2,
                Username = "bob",
                DisplayName = "Bob",
                Password = "password",
                AvatarColor = "#FF10B981"
            });
        }

        public User? SignIn(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            var u = _users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                               && x.Password == password);
            return u;
        }

        public (bool success, string error) Register(string username, string displayName, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, "Username and password required");
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return (false, "User exists");
            var id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            var user = new User
            {
                Id = id,
                Username = username,
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? username : displayName,
                Password = password,
                AvatarColor = PickColor(id)
            };
            _users.Add(user);
            return (true, string.Empty);
        }

        private string PickColor(long id)
        {
            var palette = new[]
            {
                "#FF6366F1","#FF10B981","#FFF97316","#FFEF4444","#FF06B6D4","#FF8B5CF6","#FFEC4899"
            };
            return palette[(int)(id % palette.Length)];
        }
    }
}
