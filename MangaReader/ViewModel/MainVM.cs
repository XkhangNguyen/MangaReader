using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace MangaReader.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private readonly MangaCrawler? _mangaCrawler;
        private MangaStore? _mangaStore;
        ObservableCollection<MangaModel>? _mangaModels;
        //private Timer _timer;


        private INavigationService? _navigation;

        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public MainVM(INavigationService navigationService, MangaStore mangaStore, MangaCrawler mangaCrawler)
        {
            Navigation = navigationService;
            Navigation?.NavigateTo<LoadSceneVM>();

            _mangaCrawler = mangaCrawler;
            LoadMangaDataAsync();
            _mangaStore = mangaStore;

            //_timer = new Timer(state => LoadMangaDataAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

        }
        private async void LoadMangaDataAsync()
        {
            if (_mangaCrawler != null)
            {
                _mangaModels = await _mangaCrawler.CrawlNewUpdatedManga();

                _mangaStore?.GetMangas(_mangaModels);

                Navigation?.NavigateTo<MangasDisplayVM>();

            }
        }
    }
}
