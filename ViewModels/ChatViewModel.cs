// C:\Users\St\MessengerWithTasks\MessengerApp\ViewModels\ChatViewModel.cs
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
        private readonly AssistantService _assistant;
        private readonly TaskService _taskService;

        public User? CurrentUser { get; set; }

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

        public IRelayCommand SendMessageCommand { get; }

        public ChatViewModel(ChatService chatService, AssistantService assistant, TaskService taskService)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _assistant = assistant ?? throw new ArgumentNullException(nameof(assistant));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            SendMessageCommand = new RelayCommand(SendMessage, () => !string.IsNullOrWhiteSpace(NewMessage));
        }

        public void LoadChat(string chatId)
        {
            Messages.Clear();
            var msgs = _chatService.GetMessages(chatId ?? string.Empty) ?? Enumerable.Empty<Message>();
            foreach (var m in msgs) Messages.Add(m);
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage) || CurrentUser == null) return;

            var chatId = SelectedChatIdOrDefault();

            var msg = new Message
            {
                ChatId = chatId,
                Sender = CurrentUser.DisplayName,
                Content = NewMessage,
                Timestamp = DateTime.Now,
                IsSentByMe = true
            };

            _chatService.AddMessage(msg);
            Messages.Add(msg);

            var lower = NewMessage.ToLowerInvariant();
            if (lower.Contains("помощь") || lower.Contains("help") || lower.Contains("суммируй") || lower.Contains("создай задачу") || lower.Contains("create task"))
            {
                var context = Messages.Select(m => m.Content).TakeLast(8).ToList();
                var taskTitles = _task_service_titles();

                var result = _assistant.HandleQuery(NewMessage, context, taskTitles);
                var resp = result.response;
                var newTask = result.newTask;

                var reply = new Message { ChatId = chatId, Sender = "Assistant", Content = resp, Timestamp = DateTime.Now, IsSentByMe = false };
                _chatService.AddMessage(reply);
                Messages.Add(reply);

                if (newTask != null)
                {
                    _taskService.AddTask(newTask);
                }
            }

            NewMessage = string.Empty;
        }

        private string SelectedChatIdOrDefault()
        {
            return "default";
        }

        private System.Collections.Generic.IEnumerable<string> _task_service_titles()
        {
            return _taskService.GetTasks().Select(t => t.Title);
        }
    }
}
