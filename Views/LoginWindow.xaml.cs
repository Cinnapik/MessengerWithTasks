using MessengerApp.Services;
using MessengerApp.ViewModels;
using System.Windows;

namespace MessengerApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _auth;
        private readonly StorageService _storage;

        public LoginWindow()
        {
            InitializeComponent();
            _auth = new AuthService();
            _storage = new StorageService();

            LoginBtn.Click += LoginBtn_Click;
            RegisterBtn.Click += RegisterBtn_Click;
        }

        private void RegisterBtn_Click(object? sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;
            var username = UsernameBox.Text?.Trim() ?? string.Empty;
            var password = PasswordBox.Password ?? string.Empty;
            var (ok, err) = _auth.Register(username, username, password);
            if (!ok)
            {
                ErrorText.Text = err;
                ErrorText.Visibility = Visibility.Visible;
                return;
            }
            var user = _auth.SignIn(username, password);
            if (user == null)
            {
                ErrorText.Text = "Не удалось войти после регистрации";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }
            OpenMainWindow(user);
        }

        private void LoginBtn_Click(object? sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;
            var username = UsernameBox.Text?.Trim() ?? string.Empty;
            var password = PasswordBox.Password ?? string.Empty;
            var user = _auth.SignIn(username, password);
            if (user == null)
            {
                ErrorText.Text = "Неверное имя пользователя или пароль";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }
            OpenMainWindow(user);
        }

        private void OpenMainWindow(Models.User user)
        {
            var storage = _storage;
            var chatService = new ChatService(storage);
            var taskService = new TaskService(storage);
            var assistant = new AssistantService();
            var mainVm = new MainViewModel(chatService, taskService, assistant, user);

            var main = new MainWindow { DataContext = mainVm };
            Application.Current.MainWindow = main;
            main.Show();
            this.Close();
        }
    }
}
