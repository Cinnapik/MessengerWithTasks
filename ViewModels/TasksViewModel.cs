// C:\Users\St\MessengerWithTasks\MessengerApp\ViewModels\TasksViewModel.cs
// ViewModel задач, загружает список задач и добавляет новые
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessengerApp.Models;
using MessengerApp.Services;
using System.Collections.ObjectModel;

namespace MessengerApp.ViewModels
{
    public class TasksViewModel : ObservableObject
    {
        private readonly TaskService _taskService;
        public ObservableCollection<TaskItem> Tasks { get; } = new();
        private string _newTaskTitle = string.Empty;
        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set => SetProperty(ref _newTaskTitle, value);
        }

        public IRelayCommand AddTaskCommand { get; }

        public TasksViewModel(TaskService taskService)
        {
            _taskService = taskService ?? throw new System.ArgumentNullException(nameof(taskService));
            LoadTasks();
            AddTaskCommand = new RelayCommand(AddTask);
        }

        private void LoadTasks()
        {
            Tasks.Clear();
            foreach (var t in _taskService.GetTasks()) Tasks.Add(t);
        }

        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle)) return;
            var t = new TaskItem { Title = NewTaskTitle, IsDone = false };
            _taskService.AddTask(t);
            LoadTasks();
            NewTaskTitle = string.Empty;
        }
    }
}
