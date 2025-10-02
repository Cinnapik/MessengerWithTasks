// C:\Users\St\MessengerWithTasks\MessengerApp\ViewModels\AssistantViewModel.cs
// ViewModel помощника собирает контекст из чата и задач и запрашивает ответ
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessengerApp.Services;
using System.Linq;

namespace MessengerApp.ViewModels
{
    public class AssistantViewModel : ObservableObject
    {
        private readonly AssistantService _assistant;
        private readonly ChatViewModel _chatVm;
        private readonly TasksViewModel _tasksVm;

        private string _query = string.Empty;
        public string Query
        {
            get => _query;
            set => SetProperty(ref _query, value);
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set => SetProperty(ref _response, value);
        }

        public IRelayCommand AskCommand { get; }

        public AssistantViewModel(AssistantService assistant, ChatViewModel chatVm, TasksViewModel tasksVm)
        {
            _assistant = assistant;
            _chatVm = chatVm;
            _tasksVm = tasksVm;
            AskCommand = new RelayCommand(Ask);
        }

        private void Ask()
        {
            var recent = _chatVm.Messages.Select(m => m.Content).ToList();
            var tasks = _tasksVm.Tasks.Select(t => t.Title).ToList();
            Response = _assistant.GetResponse(Query, recent, tasks);
        }
    }
}
