using MessengerApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApp.Services
{
    public class TaskService
    {
        private readonly StorageService _storage;

        public TaskService(StorageService storage)
        {
            _storage = storage;
        }

        public IEnumerable<TaskItem> GetTasks() => _storage.Tasks.ToList();

        public void AddTask(TaskItem t)
        {
            t.Id = (_storage.Tasks.Count > 0) ? _storage.Tasks.Max(x => x.Id) + 1 : 1;
            _storage.Tasks.Add(t);
        }
    }
}
