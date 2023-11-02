using System.Collections.ObjectModel;
using MangaReader.Stores;
using MangaReader.Utilities;
using MangaReader.Services;
using MangaReader.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaReader.ViewModel
{
    internal class MangasDisplayVM : ViewModelBase
    {
        private readonly MangaStore _mangaStore;

        private readonly GenreStore _genreStore;

        private readonly MangaService _mangaService;

        private IDictionary<GenreModel, ObservableCollection<MangaModel>> mangasOfGenre = new Dictionary<GenreModel, ObservableCollection<MangaModel>>();

        public RelayCommand<MangaModel> ShowMangaDetailCommand { get; }
        public RelayCommand<GenreModel> ShowMangasOfGenreCommand { get; }

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

        bool isAllManga = true;

        public MangasDisplayVM(INavigationService navigationService, MangaStore mangaStore, GenreStore genreStore, MangaService mangaService)
        {
            Navigation = navigationService;

            _mangaStore = mangaStore;

            _genreStore = genreStore;
            
            _mangaService = mangaService;

            _mangaStore.MangasListCreated += OnMangasListCreated;

            _genreStore.GenresListCreated += OnGenresListCreated;

            ShowMangaDetailCommand = new RelayCommand<MangaModel>(ShowMangaDetail);

            ShowMangasOfGenreCommand = new RelayCommand<GenreModel>(ShowMangasOfGenre!);

        }

        private void OnGenresListCreated(ObservableCollection<GenreModel> genreModels)
        {
            GenreModels = genreModels;
        }

        private void OnMangasListCreated(ObservableCollection<MangaModel> mangaModels)
        {
           isAllManga = true;

           MangaModels = mangaModels;
        }

        private async Task<ObservableCollection<MangaModel>> FindMangasOfGenre(GenreModel genreModel)
        {
            if (!mangasOfGenre.ContainsKey(genreModel))
            {
                ObservableCollection<MangaModel> mangaModels = await _mangaService.GetAllMangasOfGenre(genreModel.Id);

                mangasOfGenre.Add(genreModel, await _mangaService.GetAllMangasOfGenre(genreModel.Id));
            }

            return mangasOfGenre[genreModel];
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

        private async void ShowMangasOfGenre(GenreModel genreModel)
        {
            MangaModels = await FindMangasOfGenre(genreModel);
            isAllManga = false;
        }

        public async void ReloadAllManga()
        {
            if (!isAllManga)
            {
                MangaModels = await _mangaService.GetAllMangas();
                isAllManga = true;
            }
        }

        public async void SearchMangas(string keyword)
        {
            MangaModels = await _mangaService.GetSearchMangas(keyword);
            isAllManga = false;
        }
    }
}
