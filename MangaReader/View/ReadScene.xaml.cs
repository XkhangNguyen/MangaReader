using MangaReader.ViewModel;
using Newtonsoft.Json.Linq;
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
        private SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private (CancellationTokenSource cts, Task task)? state;
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
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel_PropertyChanged(viewModel, new PropertyChangedEventArgs(nameof(viewModel.ChapterModel)));
            }
        }

        private async void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the ChapterModel property changed
            if (e.PropertyName == "ChapterModel")
            {
                    // Create a new CancellationTokenSource for this operation
                    Task? task = null;

                    await semaphore.WaitAsync();

                    try
                    {
                        if (state.HasValue)
                        {
                            state.Value.cts.Cancel();
                            state.Value.cts.Dispose();

                            try
                            {
                                await state.Value.task;
                            }
                            catch (OperationCanceledException)
                            {

                            }

                            state = null;
                        }

                        var cts = new CancellationTokenSource();
                        task = RestartLoadImagesAsync(cts.Token);
                        state = (cts, task);
                    }
                    finally
                    {
                        semaphore.Release();
                    }

                    try
                    {
                        await task;
                    }
                    catch (OperationCanceledException)
                    {

                    }
                }
        }

        private async Task RestartLoadImagesAsync(CancellationToken token)
        {
            if (DataContext is ReadSceneVM viewModel)
            {
                loadedImages.Clear();
                foreach (var imageUrl in viewModel.ChapterModel?.ChapterImageURLs ?? new List<string>())
                {
                    await DownloadAndDisplayImage(imageUrl, token);
                }
            }
        }

        private async Task DownloadAndDisplayImage(string imageUrl, CancellationToken cancellationToken)
        {
            try
            {
                byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl, cancellationToken);

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
