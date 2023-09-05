using MangaReader.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MangaReader.View
{
    public partial class ReadScene : UserControl
    {
        private ObservableCollection<Image> loadedImages = new();

        public ReadScene()
        {
            InitializeComponent();
            DataContextChanged += ReadScene_DataContextChanged;

            // Bind the ListView to the loadedImages collection
            listView.ItemsSource = loadedImages;
        }

        private void ReadScene_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ReadSceneVM viewModel)
            {
                loadedImages.Clear();

                foreach (var imageUrl in viewModel.ChapterModel?.ChapterImageURLs ?? new List<string>())
                {
                    // Start downloading images in the background
                    DownloadImageInBackground(imageUrl);
                }
            }
        }

        private void DownloadImageInBackground(string imageUrl)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                Uri uri = e.Argument as Uri;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;  // Avoids dynamic proxy discovery delay
                    webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.Default);
                    try
                    {
                        byte[] imageBytes = null;

                        imageBytes = webClient.DownloadData(uri);

                        if (imageBytes == null)
                        {
                            e.Result = null;
                            return;
                        }
                        using (MemoryStream imageStream = new MemoryStream(imageBytes))
                        {
                            BitmapImage image = new BitmapImage();

                            image.BeginInit();
                            image.StreamSource = imageStream;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.EndInit();

                            image.Freeze();

                            e.Result = image;
                        }
                    }
                    catch (WebException ex)
                    {
                        // Handle the exception
                        e.Result = ex;
                    }
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Result is BitmapImage bitmapImage)
                {
                    // Create a new Image control to display the fully loaded image
                    var img = new Image();
                    img.Source = bitmapImage;

                    // Add the fully loaded image to the loadedImages collection
                    loadedImages.Add(img);
                }
                else if (e.Result is WebException webException)
                {
                    // Handle the WebException (e.g., display an error message)
                    Console.WriteLine($"WebException: {webException.Message}");
                }
            };

            worker.RunWorkerAsync(new Uri(imageUrl));
        }
    }
}
