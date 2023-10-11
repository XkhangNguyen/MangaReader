using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;

namespace MangaReader.ViewModel
{
    internal class MangaDetailVM:ViewModelBase
    {
        private MangaModel? _mangaModel;

        private readonly MangaStore _mangaStore;

        public RelayCommand<ChapterModel> ShowChapterDetailCommand { get; }

        private IChapterIteratorService _chapterIterator;

        private INavigationService? _navigation;
        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public MangaModel? MangaModel
        {
            get { return _mangaModel; }
            set { 
                _mangaModel = value;
                _chapterIterator.GetListChapters(_mangaModel?.Chapters);
                OnPropertyChanged(); 
            }
        }

        public MangaDetailVM(INavigationService navigationService, MangaStore mangaStore, IChapterIteratorService chapterIterator)
        {
            Navigation = navigationService;
            _chapterIterator = chapterIterator;
            
            _mangaStore = mangaStore;

            _mangaStore.MangaCreated += OnMangaCreated;

            ShowChapterDetailCommand = new RelayCommand<ChapterModel>(ShowChapterDetail);
        }

        private void ShowChapterDetail(ChapterModel? chapter)
        {
            int currentIndex = MangaModel.Chapters.IndexOf(chapter);
            _chapterIterator.SetCurrentChapterIndex(currentIndex, MangaModel);
        }

        private void OnMangaCreated(MangaModel mangaModel)
        {
            MangaModel = mangaModel;
        }
    }
}
