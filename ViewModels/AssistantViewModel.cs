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
        private readonly TaskService _taskService;

        private string _query = string.Empty;
        public string Query { get => _query; set => SetProperty(ref _query, value); }

        private string _response = string.Empty;
        public string Response { get => _response; set => SetProperty(ref _response, value); }

        public IRelayCommand AskCommand { get; }

        public AssistantViewModel(AssistantService assistant, ChatViewModel chatVm, TasksViewModel tasksVm, TaskService taskService)
        {
            _assistant = assistant;
            _chatVm = chatVm;
            _tasksVm = tasksVm;
            _taskService = taskService;
            AskCommand = new RelayCommand(Ask);
        }

        private void Ask()
        {
            var recent = _chatVm.Messages.Select(m => m.Content);
            var tasks = _tasksVm.Tasks.Select(t => t.Title);
            var res = _assistant.HandleQuery(Query, recent, tasks);
            Response = res.response;
            if (res.newTask != null)
            {
                _taskService.AddTask(res.newTask);
                _tasksVm.Load();
            }
        }
    }
}
