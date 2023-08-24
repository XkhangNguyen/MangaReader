using MangaReader.Data;
using MangaReader.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace MangaReader
{
    public partial class HomeScene : Window
    {
        //private readonly DbContextProvider? dbContextProvider;
        private readonly MangaCrawler? mangaCrawler;

        public HomeScene()
        {
            InitializeComponent();
            //dbContextProvider = new DbContextProvider("Server=localhost;Port=5432;Database=MangaDB;User Id=postgre;Password=905000Nxk@;\r\n");

            mangaCrawler = new MangaCrawler();

            LoadMangaDataAsync();
        }

        private async void LoadMangaDataAsync()
        {
            if(mangaCrawler != null)
            {
                List<Manga> mangaItems = await mangaCrawler.CrawlNewUpdatedManga();
                DataContext = new MangaModel(mangaItems);
            }
        }

        private void CoverImage_Loaded(object sender, RoutedEventArgs e)
        {
            Image? coverImage = sender as Image;
            if (coverImage != null && coverImage.DataContext is Manga manga)
            {
                coverImage.Source = new BitmapImage(new Uri("https:" + manga.CoverImageURL));
            }
        }

        public class MangaModel
        {
            public List<Manga> MangaItems { get; }

            public MangaModel(List<Manga> mangaItems)
            {
                MangaItems = mangaItems;
            }
        }

    }
}
