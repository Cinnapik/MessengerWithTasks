// Небольшая правка — LoadChat принимает nullable и после загрузки вызывает автопрокрутку через событие (Messages меняется)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessengerApp.Models;
using MessengerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MessengerApp.ViewModels
{
    public class ChatViewModel : ObservableObject
    {
        private readonly ChatService _chatService;
        private readonly TaskService _taskService;
        private readonly AssistantService _assistant;

        public ObservableCollection<Message> Messages { get; } = new();
        private string _newMessage = string.Empty;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                SetProperty(ref _newMessage, value);
                SendMessageCommand.NotifyCanExecuteChanged();
            }
        }

        public string CurrentChatId { get; private set; } = "default";
        public User? CurrentUser { get; set; }

        public IRelayCommand SendMessageCommand { get; }
        public IRelayCommand<string?> LoadChatCommand { get; }

        public ChatViewModel(ChatService chatService, AssistantService assistant, TaskService taskService)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _assistant = assistant ?? throw new ArgumentNullException(nameof(assistant));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));

            SendMessageCommand = new RelayCommand(SendMessage, () => !string.IsNullOrWhiteSpace(NewMessage));
            LoadChatCommand = new RelayCommand<string?>(LoadChat);
            LoadChat("default");
        }

        public void LoadChat(string? chatId)
        {
            var id = string.IsNullOrEmpty(chatId) ? "default" : chatId;
            CurrentChatId = id;
            Messages.Clear();
            var msgs = _chatService.GetMessages(id);
            foreach (var m in msgs) Messages.Add(m);
            // коллекция изменилась — внешние подписчики (MainWindow) прокрутят вниз
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage) || CurrentUser == null) return;

            var msg = new Message
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = CurrentChatId,
                Sender = CurrentUser.DisplayName,
                Content = NewMessage.Trim(),
                Timestamp = DateTime.Now,
                IsSentByMe = true
            };

            _chatService.AddMessage(msg);
            Messages.Add(msg);

            // assistant handling simplified
            var lower = NewMessage.ToLowerInvariant();
            if (lower.Contains("создай задачу"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(NewMessage, @"создай задачу\s+(.+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var title = match.Success ? match.Groups[1].Value.Trim() : "Новая задача";
                var task = new TaskItem { Title = title, IsDone = false };
                _task_service_safe()?.AddTask(task);
            }
            else if (lower.Contains("помощь") || lower.Contains("суммируй"))
            {
                var response = _assistant.HandleQuery(NewMessage, Messages.Select(m => m.Content), _task_service_safe()?.GetTasks().Select(t => t.Title) ?? Enumerable.Empty<string>());
                var reply = new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = CurrentChatId,
                    Sender = "Помощник",
                    Content = response.response,
                    Timestamp = DateTime.Now,
                    IsSentByMe = false
                };
                _chatService.AddMessage(reply);
                Messages.Add(reply);
            }

            NewMessage = string.Empty;
        }

        private TaskService? _task_service_safe() => _taskService;
    }
}
