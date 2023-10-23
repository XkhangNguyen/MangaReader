using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace MangaReader.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private MangaStore? _mangaStore;

        private readonly HttpClient _httpClient = new HttpClient();


        //private Timer _timer;

        public ICommand NavigateBackCommand { get;}
        public ICommand MangaDisplayCommand { get;}

        private INavigationService? _navigation;
        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private MangaService _mangaService;

        public MainVM(INavigationService navigationService, MangaStore mangaStore, MangaService mangaService)
        {
            Navigation = navigationService;
            _mangaService = mangaService;

            Navigation?.NavigateTo<LoadSceneVM>();

            _mangaStore = mangaStore;

            LoadMangaDataAsync();
            //_timer = new Timer(state => LoadMangaDataAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(15));


            NavigateBackCommand = new RelayCommand<ViewModelBase>(GoBack);
            MangaDisplayCommand = new RelayCommand<ViewModelBase>(GoToMangaDisplay);

        }

        private void GoToMangaDisplay(ViewModelBase? @base)
        {
            Navigation?.NavigateTo<MangasDisplayVM>();
        }

        private async Task LoadMangaDataAsync()
        {
            ObservableCollection<MangaModel> MangaList = null;

            // Define the maximum number of retries
            int maxRetries = 3;
            int retryCount = 0;

            while (MangaList == null && retryCount < maxRetries)
            {
                try
                {
                    MangaList = await _mangaService.GetAllMangas();
                }
                catch (Exception ex)
                {
                    // Log the error or handle it as appropriate
                    Console.WriteLine($"Error while getting Manga data: {ex.Message}");

                    // Increment the retry count
                    retryCount++;

                    // Add a delay before retrying (exponential backoff)
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)));
                }
            }

            if (MangaList != null)
            {
                _mangaStore?.FetchMangasData(MangaList);
                Navigation?.NavigateTo<MangasDisplayVM>();
            }
            else
            {
                // Handle the case where data could not be retrieved after maxRetries
                Console.WriteLine("Failed to retrieve Manga data after maximum retries.");
            }
        }


        private void GoBack(ViewModelBase viewModelBase)
        {
            Navigation?.NavigateBack();
        }
    }
}
