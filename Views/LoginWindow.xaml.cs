using MessengerApp.Services;
using MessengerApp.ViewModels;
using System;
using System.IO;
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
            try
            {
                ErrorText.Visibility = Visibility.Collapsed;

                var username = UsernameBox?.Text?.Trim() ?? string.Empty;
                var password = PasswordBox?.Password ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ShowError("Введите имя пользователя и пароль");
                    return;
                }

                object resultObj;
                try
                {
                    // Вызываем Register и получаем результат как object
                    resultObj = _auth.Register(username, username, password)!;
                }
                catch (Exception ex)
                {
                    HandleUnexpectedError(ex);
                    return;
                }

                // Безопасно извлекаем булево значение успеха и текст ошибки
                bool ok = true;
                string? err = null;

                // 1) если Register вернул bool
                if (resultObj is bool b)
                {
                    ok = b;
                }
                else if (resultObj is ValueTuple<bool, string> vt)
                {
                    ok = vt.Item1;
                    err = vt.Item2;
                }
                else
                {
                    // 2) попытка получить свойства Item1/Item2 через reflection (поддержка анонимных/tuple-like типов)
                    try
                    {
                        var type = resultObj?.GetType();
                        var prop1 = type?.GetProperty("Item1");
                        var prop2 = type?.GetProperty("Item2");
                        if (prop1 != null)
                        {
                            var val1 = prop1.GetValue(resultObj);
                            if (val1 is bool vb) ok = vb;
                        }
                        if (prop2 != null)
                        {
                            var val2 = prop2.GetValue(resultObj);
                            if (val2 != null) err = val2.ToString();
                        }
                    }
                    catch
                    {
                        // если ничего не удалось извлечь — считаем, что регистрация успешна
                        ok = true;
                    }
                }

                if (!ok)
                {
                    ShowError(string.IsNullOrEmpty(err) ? "Не удалось зарегистрироваться" : err);
                    return;
                }

                var user = _auth.SignIn(username, password);
                if (user == null)
                {
                    ShowError("Не удалось войти после регистрации");
                    return;
                }

                OpenMainWindowSafe(user);
            }
            catch (Exception ex)
            {
                HandleUnexpectedError(ex);
            }
        }

        private void LoginBtn_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                ErrorText.Visibility = Visibility.Collapsed;

                var username = UsernameBox?.Text?.Trim() ?? string.Empty;
                var password = PasswordBox?.Password ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ShowError("Введите имя пользователя и пароль");
                    return;
                }

                var user = _auth.SignIn(username, password);
                if (user == null)
                {
                    ShowError("Неверное имя пользователя или пароль");
                    return;
                }

                OpenMainWindowSafe(user);
            }
            catch (Exception ex)
            {
                HandleUnexpectedError(ex);
            }
        }

        private void OpenMainWindowSafe(Models.User user)
        {
            try
            {
                if (user == null)
                {
                    ShowError("Пользователь не найден");
                    return;
                }

                var storage = _storage ?? new StorageService();
                var chatService = new ChatService(storage);
                var taskService = new TaskService(storage);
                var assistant = new AssistantService();
                var mainVm = new MainViewModel(chatService, taskService, assistant, user);

                var main = new MainWindow { DataContext = mainVm };
                Application.Current.MainWindow = main;
                main.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                HandleUnexpectedError(ex);
            }
        }

        private void ShowError(string message)
        {
            ErrorText.Text = message;
            ErrorText.Visibility = Visibility.Visible;
        }

        private void HandleUnexpectedError(Exception ex)
        {
            try
            {
                var msg = "Произошла ошибка. Подробности записаны в run_error.log";
                MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                var exeDir = AppDomain.CurrentDomain.BaseDirectory ?? Directory.GetCurrentDirectory();
                var logFile = Path.Combine(exeDir, "run_error.log");
                var text = $"Time: {DateTime.Now:O}\nException: {ex}\n";
                File.AppendAllText(logFile, text);
            }
            catch
            {
                // ничего не делаем
            }
        }
    }
}
