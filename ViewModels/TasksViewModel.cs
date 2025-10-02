// C:\Users\St\MessengerWithTasks\MessengerApp\ViewModels\TasksViewModel.cs
// Модель представления задач, поддерживает добавление, редактирование, удаление и переключение состояния
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessengerApp.Models;
using MessengerApp.Services;
using System;
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

        private TaskItem? _editingTask;
        public TaskItem? EditingTask
        {
            get => _editingTask;
            set => SetProperty(ref _editingTask, value);
        }

        public IRelayCommand AddTaskCommand { get; }
        public IRelayCommand<TaskItem?> EditTaskCommand { get; }
        public IRelayCommand<TaskItem?> DeleteTaskCommand { get; }
        public IRelayCommand<TaskItem?> ToggleDoneCommand { get; }
        public IRelayCommand SaveEditCommand { get; }
        public IRelayCommand CancelEditCommand { get; }

        public TasksViewModel(TaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            AddTaskCommand = new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand<TaskItem?>(BeginEdit);
            DeleteTaskCommand = new RelayCommand<TaskItem?>(DeleteTask);
            ToggleDoneCommand = new RelayCommand<TaskItem?>(ToggleDone);
            SaveEditCommand = new RelayCommand(SaveEdit);
            CancelEditCommand = new RelayCommand(CancelEdit);
            LoadTasks();
        }

        private void LoadTasks()
        {
            Tasks.Clear();
            foreach (var t in _taskService.GetTasks()) Tasks.Add(t);
        }

        public void Reload() => LoadTasks();

        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle)) return;
            var t = new TaskItem { Title = NewTaskTitle.Trim(), IsDone = false };
            _taskService.AddTask(t);
            NewTaskTitle = string.Empty;
            LoadTasks();
        }

        private void BeginEdit(TaskItem? item)
        {
            if (item == null) return;
            EditingTask = new TaskItem { Id = item.Id, Title = item.Title, IsDone = item.IsDone };
        }

        private void SaveEdit()
        {
            if (EditingTask == null) return;
            _taskService.UpdateTask(EditingTask);
            EditingTask = null;
            LoadTasks();
        }

        private void CancelEdit()
        {
            EditingTask = null;
        }

        private void DeleteTask(TaskItem? item)
        {
            if (item == null) return;
            _taskService.DeleteTask(item.Id);
            LoadTasks();
        }

        private void ToggleDone(TaskItem? item)
        {
            if (item == null) return;
            item.IsDone = !item.IsDone;
            _taskService.UpdateTask(item);
            LoadTasks();
        }
    }
}
