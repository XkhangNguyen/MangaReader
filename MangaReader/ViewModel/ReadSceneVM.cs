using MangaReader.Model;
using MangaReader.Stores;
using MangaReader.Utilities;

namespace MangaReader.ViewModel
{
    public class ReadSceneVM : ViewModelBase
    {
        private readonly MangaStore _mangaStore;
        private readonly MangaCrawler _mangaCrawler;

        private ChapterModel? _chapterModel;
        public ChapterModel? ChapterModel
        {
            get { return _chapterModel; }
            set { _chapterModel = value; OnPropertyChanged(); }
        }

        public ReadSceneVM(MangaStore mangaStore, MangaCrawler mangaCrawler) 
        {
            _mangaStore = mangaStore;
            _mangaStore.ChapterCreated += OnChapterCreated;

            _mangaCrawler = mangaCrawler;
        }

        private async void OnChapterCreated(ChapterModel chapterModel)
        {
            ChapterModel = chapterModel;
            ChapterModel.ChapterImageURLs = await _mangaCrawler.CrawlChapterImgUrlAsync(chapterModel);
        }

        public override void Dispose()
        {
            _mangaStore.ChapterCreated -= OnChapterCreated;

            base.Dispose();
        }
    }
}
