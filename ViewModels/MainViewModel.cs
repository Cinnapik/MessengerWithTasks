// C:\Users\St\MessengerWithTasks\MessengerApp\ViewModels\MainViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessengerApp.Models;
using MessengerApp.Services;
using System.Collections.ObjectModel;

namespace MessengerApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<string> Chats { get; } = new();
        private string _selectedChat = string.Empty;
        public string NewChatName { get; set; } = string.Empty;

        private readonly ChatService _chatService;
        private readonly TaskService _taskService;
        private readonly AssistantService _assistantService;

        public ChatViewModel ChatVM { get; }
        public TasksViewModel TasksVM { get; }
        public AssistantViewModel AssistantVM { get; }

        public IRelayCommand NewChatCommand { get; }
        public IRelayCommand SignOutCommand { get; }

        public User CurrentUser { get; }

        public string SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                if (!string.IsNullOrEmpty(value)) ChatVM.LoadChat(value);
            }
        }

        public MainViewModel(ChatService chatService, TaskService taskService, AssistantService assistant, User user)
        {
            _chatService = chatService ?? throw new System.ArgumentNullException(nameof(chatService));
            _task_service_guard(taskService);
            _taskService = taskService ?? throw new System.ArgumentNullException(nameof(taskService));
            _assistantService = assistant ?? throw new System.ArgumentNullException(nameof(assistant));
            CurrentUser = user ?? throw new System.ArgumentNullException(nameof(user));

            ChatVM = new ChatViewModel(_chatService, _assistantService, _taskService) { CurrentUser = CurrentUser };
            TasksVM = new TasksViewModel(_taskService);
            AssistantVM = new AssistantViewModel(_assistantService, ChatVM, TasksVM, _taskService);

            Chats.Add("default");
            Chats.Add("work");
            Chats.Add("random");
            SelectedChat = "default";

            NewChatCommand = new RelayCommand(CreateChat);
            SignOutCommand = new RelayCommand(SignOut);
        }

        private void CreateChat()
        {
            var name = string.IsNullOrWhiteSpace(NewChatName) ? ("чат-" + (Chats.Count + 1)) : NewChatName.Trim();
            Chats.Add(name);
            SelectedChat = name;
            NewChatName = string.Empty;
        }

        private void _task_service_guard(TaskService ts)
        {
            // простая проверка на null, оставлена для безопасности
            if (ts == null) throw new System.ArgumentNullException(nameof(ts));
        }
 
        private void SignOut()
        {
            var wnd = System.Windows.Application.Current.MainWindow;
            var login = new Views.LoginWindow();
            login.Show();
            if (wnd != null) wnd.Close();
        }
    }
}
