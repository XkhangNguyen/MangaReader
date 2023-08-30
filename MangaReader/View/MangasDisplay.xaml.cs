using MangaReader.Model;
using MangaReader.Stores;
using MangaReader.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace MangaReader.View
{
    /// <summary>
    /// Interaction logic for MangasDisplay.xaml
    /// </summary>
    public partial class MangasDisplay : UserControl
    {
        public MangasDisplay()
        {
            InitializeComponent();
        }

        private void CoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image coverImage && coverImage.DataContext is MangaModel manga)
            {
                coverImage.Source = new BitmapImage(new Uri("https:" + manga.CoverImageURL));
            }
        }
    }
}
