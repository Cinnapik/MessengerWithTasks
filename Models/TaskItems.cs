// C:\Users\St\MessengerWithTasks\MessengerApp\Models\TaskItem.cs
// Модель задачи
namespace MessengerApp.Models
{
    public class TaskItem
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
