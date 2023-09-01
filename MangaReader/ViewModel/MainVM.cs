using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Ultilities;
using MangaReader.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangaReader.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private readonly OtakuSancCrawler? _mangaCrawler;
        private MangaStore? _mangaStore;
        ObservableCollection<MangaModel>? _mangaModels;
        //private Timer _timer;

        public ICommand NavigateBackCommand { get;}

        private INavigationService? _navigation;
        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public MainVM(INavigationService navigationService, MangaStore mangaStore, OtakuSancCrawler mangaCrawler)
        {
            Navigation = navigationService;
            Navigation?.NavigateTo<LoadSceneVM>();

            _mangaCrawler = mangaCrawler;
            _mangaStore = mangaStore;

            LoadMangaDataAsync();
            //_timer = new Timer(state => LoadMangaDataAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

            NavigateBackCommand = new RelayCommand<ViewModelBase>(GoBack);
        }

        private void LoadMangaDataAsync()
        {
            if (_mangaCrawler != null)
            {
                _mangaModels = new ObservableCollection<MangaModel>();
                string filePath = @"C:\Users\Admin\Desktop\Projects\MangaReader\MangaScraper\results.json";
                foreach (var manga in JsonMangaReader.ReadJsonFile(filePath))
                {
                    _mangaModels.Add(manga);
                }

                _mangaStore?.GetMangas(_mangaModels);

                Navigation?.NavigateTo<MangasDisplayVM>();
            }
        }

        private void GoBack(ViewModelBase viewModelBase)
        {
            Navigation?.NavigateBack();
        }
    }
}
