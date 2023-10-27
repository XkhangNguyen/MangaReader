using MangaReader.Model;
using MangaReader.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace MangaReader.View
{
    /// <summary>
    /// Interaction logic for MangaDetail.xaml
    /// </summary>
    public partial class MangaDetail : UserControl
    {
        public MangaDetail()
        {
            InitializeComponent();
        }

        private void CoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image coverImage && coverImage.DataContext is MangaModel manga)
            {
                coverImage.Source = new BitmapImage(new Uri("https:" + manga.CoverImageUrl));
            }
        }

        private void UserControl_PreviewButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MangaDetailVM viewModel)
            {
                if (e.ChangedButton == MouseButton.XButton1)
                {
                    viewModel.ToMangaDisplay();
                    e.Handled = true; // Mark the event as handled to prevent other keyboard shortcuts from interfering
                }
            }
        }
    }
}
