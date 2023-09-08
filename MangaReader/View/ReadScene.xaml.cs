using MangaReader.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MangaReader.View
{
    public partial class ReadScene : UserControl
    {
        private ObservableCollection<Image> loadedImages = new();
        private HttpClient httpClient = new HttpClient();
        private CancellationTokenSource? cancellationTokenSource;

        public ReadScene()
        {
            InitializeComponent();
            DataContextChanged += ReadScene_DataContextChanged;

            // Bind the ListView to the loadedImages collection
            listView.ItemsSource = loadedImages;
        }

        private async void ReadScene_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ReadSceneVM viewModel)
            {
                // Cancel any ongoing tasks
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                }
                cancellationTokenSource = new();

                viewModel.PropertyChanged += ViewModel_PropertyChanged;
                loadedImages.Clear();

                foreach (var imageUrl in viewModel.ChapterModel?.ChapterImageURLs ?? new List<string>())
                {
                    // Start downloading and displaying images in the correct order
                    await DownloadAndDisplayImage(imageUrl);
                }
            }
        }

        private async void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the ChapterModel property changed
            if (e.PropertyName == "ChapterModel")
            {
                if (DataContext is ReadSceneVM viewModel)
                {
                    // Cancel any ongoing tasks when ChapterModel changes
                    if (cancellationTokenSource != null)
                    {
                        cancellationTokenSource.Cancel();
                        cancellationTokenSource.Dispose();
                    }

                    // Create a new CancellationTokenSource for this operation
                    cancellationTokenSource = new();
                    loadedImages.Clear();

                    // Re-populate images when ChapterModel changes
                    foreach (var imageUrl in viewModel.ChapterModel?.ChapterImageURLs ?? new List<string>())
                    {
                        await DownloadAndDisplayImage(imageUrl);
                    }
                }
            }
        }

        private async Task DownloadAndDisplayImage(string imageUrl)
        {
            try
            {
                byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl, cancellationTokenSource.Token);
                
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                using (MemoryStream imageStream = new MemoryStream(imageBytes))
                {
                    BitmapImage image = new();

                    image.BeginInit();
                    image.StreamSource = imageStream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    image.Freeze();

                    // Dispatch to the UI thread to add the fully loaded image to the list
                    Dispatcher.Invoke(() =>
                    {
                        // Create a new Image control to display the fully loaded image
                        var img = new Image();
                        img.Source = image;

                        // Add the fully loaded image to the list
                        loadedImages.Add(img);
                    });
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle the exception (e.g., display an error message)
                Console.WriteLine($"HttpRequestException: {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                // Operation was canceled (e.g., when ChapterModel changes)
            }
        }
    }
}
