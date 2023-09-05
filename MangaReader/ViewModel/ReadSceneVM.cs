using MangaReader.Model;
using MangaReader.Stores;
using MangaReader.Utilities;

namespace MangaReader.ViewModel
{
    public class ReadSceneVM : ViewModelBase
    {
        private readonly MangaStore _mangaStore;

        private ChapterModel? _chapterModel;
        public ChapterModel? ChapterModel
        {
            get { return _chapterModel; }
            set { _chapterModel = value; OnPropertyChanged(); }
        }

        public ReadSceneVM(MangaStore mangaStore) 
        {
            _mangaStore = mangaStore;
            _mangaStore.ChapterCreated += OnChapterCreated;
        }

        private void OnChapterCreated(ChapterModel chapterModel)
        {
            ChapterModel = chapterModel;
        }

        public override void Dispose()
        {
            _mangaStore.ChapterCreated -= OnChapterCreated;

            base.Dispose();
        }
    }
}
