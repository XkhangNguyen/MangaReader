using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MangaReader.Utilities
{
    public class ChaptersToColumnCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int chapterCount)
            {
                int columns = (int)Math.Ceiling((double)chapterCount / 8);
                return columns;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
