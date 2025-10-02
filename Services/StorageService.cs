// C:\Users\St\MessengerWithTasks\MessengerApp\Services\StorageService.cs
// Управление локальной SQLite базой
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace MessengerApp.Services
{
    public class StorageService
    {
        private readonly string _dbPath;

        public StorageService()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MessengerApp");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            _dbPath = Path.Combine(folder, "app.db");
            Initialize();
        }

        private void Initialize()
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Messages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ChatId TEXT,
                Content TEXT,
                Timestamp TEXT,
                IsSentByMe INTEGER
            );
            CREATE TABLE IF NOT EXISTS Tasks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT,
                IsDone INTEGER
            );";
            cmd.ExecuteNonQuery();
        }

        public SqliteConnection GetConnection() => new SqliteConnection($"Data Source={_dbPath}");
    }
}
