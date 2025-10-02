using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MessengerApp.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isMine = value is bool b && b;
            return isMine
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0EA5A4")) 
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1F2937"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            DependencyProperty.UnsetValue;
    }
}
