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

        private readonly ChatService _chatService;
        private readonly TaskService _taskService;
        private readonly AssistantService _assistantService;

        public User CurrentUser { get; }

        public string TitleBarDisplay => $"{CurrentUser.DisplayName} • Messenger";

        public string SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                if (!string.IsNullOrEmpty(value)) ChatVM.LoadChat(value);
            }
        }

        public ChatViewModel ChatVM { get; }
        public TasksViewModel TasksVM { get; }
        public AssistantViewModel AssistantVM { get; }

        public IRelayCommand NewChatCommand { get; }
        public IRelayCommand SignOutCommand { get; }

        public MainViewModel(ChatService chatService, TaskService taskService, AssistantService assistant, User currentUser)
        {
            _chatService = chatService ?? throw new System.ArgumentNullException(nameof(chatService));
            _taskService = taskService ?? throw new System.ArgumentNullException(nameof(taskService));
            _assistantService = assistant ?? throw new System.ArgumentNullException(nameof(assistant));
            CurrentUser = currentUser ?? throw new System.ArgumentNullException(nameof(currentUser));

            ChatVM = new ChatViewModel(_chatService, _assistantService, _taskService) { CurrentUser = CurrentUser };
            TasksVM = new TasksViewModel(_taskService);
            AssistantVM = new AssistantViewModel(_assistantService, ChatVM, TasksVM, _taskService);

            Chats.Add("Общий");
            Chats.Add("Разработка");
            Chats.Add("Личные");
            SelectedChat = Chats[0];

            NewChatCommand = new RelayCommand(() =>
            {
                var name = "Чат " + (Chats.Count + 1);
                Chats.Add(name);
                SelectedChat = name;
            });

            SignOutCommand = new RelayCommand(SignOut);
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
