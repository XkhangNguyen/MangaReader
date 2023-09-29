using MangaReader.Model;
using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;

namespace MangaReader.ViewModel
{
    internal class MangaDetailVM:ViewModelBase
    {
        private Manga? _manga;

        private readonly MangaStore _mangaStore;

        public RelayCommand<Chapter> ShowChapterDetailCommand { get; }

        private IChapterIteratorService _chapterIterator;

        private INavigationService? _navigation;
        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public Manga? Manga
        {
            get { return _manga; }
            set { 
                _manga = value;
                _chapterIterator.GetListChapters(_manga?.Chapters);
                OnPropertyChanged(); 
            }
        }

        public MangaDetailVM(INavigationService navigationService, MangaStore mangaStore, IChapterIteratorService chapterIterator)
        {
            Navigation = navigationService;
            _chapterIterator = chapterIterator;
            
            _mangaStore = mangaStore;

            _mangaStore.MangaCreated += OnMangaCreated;

            ShowChapterDetailCommand = new RelayCommand<Chapter>(ShowChapterDetail);
        }

        private void ShowChapterDetail(Chapter? chapter)
        {
            int currentIndex = Manga.Chapters.IndexOf(chapter);
            _chapterIterator.SetCurrentChapterIndex(currentIndex, Manga);
        }

        private void OnMangaCreated(Manga mangaModel)
        {
            Manga = mangaModel;
        }
    }
}
