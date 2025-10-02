// C:\Users\St\MessengerWithTasks\MessengerApp\Services\TaskService.cs
// Сервис работы с задачами
using MessengerApp.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace MessengerApp.Services
{
    public class TaskService
    {
        private readonly StorageService _storage;
        public TaskService(StorageService storage) => _storage = storage;

        public IEnumerable<TaskItem> GetTasks()
        {
            var list = new List<TaskItem>();
            using var conn = _storage.GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, IsDone FROM Tasks";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new TaskItem
                {
                    Id = rdr.GetInt64(0),
                    Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    IsDone = rdr.GetInt32(2) == 1
                });
            }
            return list;
        }

        public void AddTask(TaskItem t)
        {
            using var conn = _storage.GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Tasks (Title, IsDone) VALUES ($title, $isdone)";
            cmd.Parameters.AddWithValue("$title", t.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("$isdone", t.IsDone ? 1 : 0);
            cmd.ExecuteNonQuery();
        }

        public void UpdateTask(TaskItem t)
        {
            using var conn = _storage.GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Tasks SET Title = $title, IsDone = $isdone WHERE Id = $id";
            cmd.Parameters.AddWithValue("$title", t.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("$isdone", t.IsDone ? 1 : 0);
            cmd.Parameters.AddWithValue("$id", t.Id);
            cmd.ExecuteNonQuery();
        }
    }
}
