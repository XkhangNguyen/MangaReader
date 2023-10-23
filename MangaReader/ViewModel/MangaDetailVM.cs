using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using System.Collections.ObjectModel;


namespace MangaReader.ViewModel
{
    internal class MangaDetailVM:ViewModelBase
    {
        private MangaModel? _mangaModel;

        private readonly MangaStore _mangaStore;
        private readonly MangaService _mangaService;

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

                LoadMangaChaptersAsync();

                OnPropertyChanged(); 
            }
        }

        private async void LoadMangaChaptersAsync()
        {
            if (_mangaModel != null)
            {

                if(_mangaModel.chaptersFetched == false)
                {
                    var chapters = await _mangaService.GetMangaChapters(_mangaModel.Id);

                    foreach (var chapter in chapters)
                    {
                        MangaModel!.Chapters.Add(chapter);
                    }

                    _mangaModel.chaptersFetched = true;

                    _chapterIterator.GetListChapters(chapters); 
                }
                else
                {
                    _chapterIterator.GetListChapters(_mangaModel.Chapters);
                }


            }
        }

        public MangaDetailVM(INavigationService navigationService, MangaStore mangaStore, IChapterIteratorService chapterIterator, MangaService mangaService)
        {
            Navigation = navigationService;

            _chapterIterator = chapterIterator;
            
            _mangaStore = mangaStore;

            _mangaService = mangaService;

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
