using MangaReader.Model;
using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MangaReader.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private MangaStore? _mangaStore;

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

        private async void LoadMangaDataAsync()
        {
            
            _mangaStore?.GetMangas(await _mangaService.GetAllMangas());

            Navigation?.NavigateTo<MangasDisplayVM>();
        }

        private void GoBack(ViewModelBase viewModelBase)
        {
            Navigation?.NavigateBack();
        }
    }
}
