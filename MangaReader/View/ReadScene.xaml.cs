using MangaReader.Model;
using MangaReader.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void ReadScene_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
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
                comboBox.SelectedItem = viewModel.ChapterModel;
                scroll.ScrollToVerticalOffset(0);
                scroll.Focus();
                loadedImages.Clear();

                await viewModel.LoadImagesAsync();

                foreach (var imageUrl in viewModel.ChapterModel?.ChapterImageURLs!)
                {
                   await DownloadAndDisplayImage("https://i.pinimg.com/236x/5e/c9/52/5ec95263d4b1f43da42529206add991c.jpg", token);
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ReadSceneVM viewModel)
            {
                if (comboBox.SelectedItem is ChapterModel selectedChapter)
                {
                    selectedChapter.MangaModel = viewModel.ChapterIterator.CurrentManga;
                    int currentIndex = selectedChapter.MangaModel.Chapters!.IndexOf(selectedChapter);
                    viewModel.ChapterIterator.SetCurrentChapterIndex(currentIndex, selectedChapter.MangaModel);
                }
            }
        }

        private void ScrollView_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                // Synchronize the ComboBox's Y position with the ScrollViewer's vertical offset
                buttonsPanel.Margin = new Thickness(0, 10 , 0, 0);
            }

            if(e.VerticalOffset == 0)
            {
                buttonsPanel.Margin = new Thickness(0, 100, 0, 0);
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is ReadSceneVM viewModel)
            {
                if (e.Key == Key.Left)
                {
                    viewModel.PreviousChapterCommand.Execute(null);
                    e.Handled = true; // Mark the event as handled to prevent other keyboard shortcuts from interfering
                }

                if (e.Key == Key.Right)
                {
                    viewModel.NextChapterCommand.Execute(null);
                    e.Handled = true; // Mark the event as handled to prevent other keyboard shortcuts from interfering
                }
            }
        }

        private void UserControl_PreviewButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ReadSceneVM viewModel)
            {
                if (e.ChangedButton == MouseButton.XButton1)
                {
                    viewModel.PreviousChapterCommand.Execute(null);
                    e.Handled = true; // Mark the event as handled to prevent other keyboard shortcuts from interfering
                }

                if (e.ChangedButton == MouseButton.XButton2)
                {
                    viewModel.NextChapterCommand.Execute(null);
                    e.Handled = true; // Mark the event as handled to prevent other keyboard shortcuts from interfering
                }
            }
        }
    }
}
