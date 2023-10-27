using System.Collections.ObjectModel;
using MangaReader.Stores;
using MangaReader.Utilities;
using MangaReader.Services;
using MangaReader.Model;
using System;

namespace MangaReader.ViewModel
{
    internal class MangasDisplayVM : ViewModelBase
    {
        private readonly MangaStore _mangaStore;

        private readonly GenreStore _genreStore;

        public RelayCommand<MangaModel> ShowMangaDetailCommand { get; }

        private INavigationService? _navigation;
        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MangaModel>? _mangaModels;
        public ObservableCollection<MangaModel>? MangaModels
        {
            get { return _mangaModels; }
            set
            {
                _mangaModels = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GenreModel>? _genreModels;
        public ObservableCollection<GenreModel>? GenreModels
        {
            get { return _genreModels; }
            set
            {
                _genreModels = value;
                OnPropertyChanged();
            }
        }

        public MangasDisplayVM(INavigationService navigationService, MangaStore mangaStore, GenreStore genreStore)
        {
            Navigation = navigationService;

            _mangaStore = mangaStore;

            _genreStore = genreStore;

            _mangaStore.MangasListCreated += OnMangasListCreated;

            _genreStore.GenresListCreated += OnGenresListCreated;

            ShowMangaDetailCommand = new RelayCommand<MangaModel>(ShowMangaDetail);

        }

        private void OnGenresListCreated(ObservableCollection<GenreModel> genreModels)
        {
            GenreModels = genreModels;
        }

        private void OnMangasListCreated(ObservableCollection<MangaModel> mangaModels)
        {
           MangaModels = mangaModels;
        }

        public override void Dispose()
        {
            _mangaStore.MangasListCreated -= OnMangasListCreated;
            _genreStore.GenresListCreated -= OnGenresListCreated;

            base.Dispose();
        }

        private void ShowMangaDetail(MangaModel? manga)
        {
            _mangaStore.GetManga(manga);

            Navigation?.NavigateTo<MangaDetailVM>();
        }
    }
}
