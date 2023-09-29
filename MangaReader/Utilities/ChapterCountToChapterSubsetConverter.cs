using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MangaReader.Models;

namespace MangaReader.Utilities
{
    public class ChapterCountToChapterSubsetConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value is List<Chapter> chapters)
            {
                int maxChaptersToShow = 3;
                return chapters.Take(maxChaptersToShow).ToList();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

