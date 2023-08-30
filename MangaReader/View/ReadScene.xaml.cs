using MangaReader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MangaReader.View
{
    /// <summary>
    /// Interaction logic for ReadScene.xaml
    /// </summary>
    public partial class ReadScene : UserControl
    {
        public ReadScene()
        {
            InitializeComponent();

        }

        private void CoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image coverImage && coverImage.DataContext is string url)
            {
                coverImage.Source = new BitmapImage(new Uri("https:" + url));
            }
        }
    }
}
