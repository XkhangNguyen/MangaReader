using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MangaReader.Utilities
{
    public class ScreenHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                // Calculate the screen height and subtract some padding if needed
                double screenHeight = SystemParameters.PrimaryScreenHeight - 20; // Adjust padding as needed
                return screenHeight;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
