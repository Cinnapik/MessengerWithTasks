using MessengerApp.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace MessengerApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MainViewModel vm)
            {
                if (vm.ChatVM?.Messages != null)
                {
                    vm.ChatVM.Messages.CollectionChanged -= Messages_CollectionChanged;
                    vm.ChatVM.Messages.CollectionChanged += Messages_CollectionChanged;
                }

                vm.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(vm.SelectedChat))
                    {
                        ScrollToBottom();
                    }
                };
            }
        }

        private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ScrollToBottom();
            }
        }

        private void ScrollToBottom()
        {
            var sv = this.FindName("MessagesScroll") as ScrollViewer;
            if (sv != null)
            {
                sv.Dispatcher.InvokeAsync(() => sv.ScrollToEnd());
            }
        }
    }
}
