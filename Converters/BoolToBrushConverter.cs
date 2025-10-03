using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MessengerApp.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        // true -> отправленные (светло-зелёный), false -> чужие (тёмный панельный)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isMine = value is bool b && b;
            return isMine
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF34D399"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF16202B"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            DependencyProperty.UnsetValue;
    }
}
