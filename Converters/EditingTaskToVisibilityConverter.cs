// C:\Users\St\MessengerWithTasks\MessengerApp\Converters\EditingTaskToVisibilityConverter.cs
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MessengerApp.ViewModels;
using MessengerApp.Models;

namespace MessengerApp.Converters
{
    public class EditingTaskToVisibilityConverter : IValueConverter
    {
        // parameter expected to be task Id as string, value expected TasksViewModel. Not used now, return Collapsed.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
