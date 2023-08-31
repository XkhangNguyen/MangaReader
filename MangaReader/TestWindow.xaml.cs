using MangaReader.Model;
using System;
using System.Windows;
using System.Windows.Media.Imaging;



namespace MangaReader
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            DataContext = Test();
        }

        MangaModel Test()
        {
            string filePath = @"C:\Users\Admin\Desktop\Projects\MangaReader\MangaScraper\results.json";

            var mangaModel = JsonMangaReader.ReadJsonFile(filePath)[0];
            string output = "";
            output += mangaModel.MangaTitle + "\n";
            output += mangaModel.MangaDescription + "\n";
            output += mangaModel.Author + "\n";
            output += mangaModel.CoverImageURL + "\n";

            foreach(var genre in mangaModel.Genres)
            {
                output += genre;
            }
            TestBox.Text = output;

            BitmapImage coverImage = new BitmapImage(new Uri(mangaModel.CoverImageURL));
            CoverImage.Source = coverImage;

            return mangaModel;
        }
    }
}
