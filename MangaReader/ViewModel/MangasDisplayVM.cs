using MangaReader.Models;
using System.Collections.ObjectModel;
using MangaReader.Stores;
using MangaReader.Utilities;
using MangaReader.Services;

namespace MangaReader.ViewModel
{
    internal class MangasDisplayVM : ViewModelBase
    {
        private readonly MangaStore _mangaStore;
        public RelayCommand<Manga> ShowMangaDetailCommand { get; }

        private INavigationService? _navigation;

        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Manga>? _mangas;
        public ObservableCollection<Manga>? Mangas
        {
            get { return _mangas; }
            set
            {
                _mangas = value;
                OnPropertyChanged();
            }
        }

        public MangasDisplayVM(INavigationService navigationService, MangaStore mangaStore)
        {
            Navigation = navigationService;

            _mangaStore = mangaStore;

            _mangaStore.MangasListCreated += OnMangasListCreated;

            ShowMangaDetailCommand = new RelayCommand<Manga>(ShowMangaDetail);

        }

        private void OnMangasListCreated(ObservableCollection<Manga> mangaModels)
        {
           Mangas = mangaModels;
        }

        public override void Dispose()
        {
            _mangaStore.MangasListCreated -= OnMangasListCreated;

            base.Dispose();
        }

        private void ShowMangaDetail(Manga? manga)
        {
            _mangaStore.GetManga(manga);

            Navigation?.NavigateTo<MangaDetailVM>();
        }


    }
}
