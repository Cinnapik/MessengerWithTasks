// C:\Users\St\MessengerWithTasks\MessengerApp\ViewModels\MainViewModel.cs
// Главный ViewModel, создаёт сервисы и под-VM, управляет списком чатов
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessengerApp.Services;
using System.Collections.ObjectModel;

namespace MessengerApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<string> Chats { get; } = new();
        private string _selectedChat = string.Empty;
        public string SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                if (value != null) ChatVM.LoadChat(value);
            }
        }

        public ChatViewModel ChatVM { get; }
        public TasksViewModel TasksVM { get; }
        public AssistantViewModel AssistantVM { get; }

        public IRelayCommand NewChatCommand { get; }

        public MainViewModel()
        {
            var storage = new StorageService();
            var chatService = new ChatService(storage);
            var taskService = new TaskService(storage);
            var assistant = new AssistantService();

            ChatVM = new ChatViewModel(chatService, assistant, taskService);
            TasksVM = new TasksViewModel(taskService);
            AssistantVM = new AssistantViewModel(assistant, ChatVM, TasksVM);

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
        }
    }
}
