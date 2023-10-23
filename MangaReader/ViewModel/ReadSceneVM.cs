using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Utilities;
using System;
using System.Threading.Tasks;

namespace MangaReader.ViewModel
{
    public class ReadSceneVM : ViewModelBase
    {
        private IChapterIteratorService _chapterIterator;
        private readonly MangaService _mangaService;
        public IChapterIteratorService ChapterIterator
        {
            get { return _chapterIterator; }
        }

        private ChapterModel? _chapterModel;
        public ChapterModel? ChapterModel
        {
            get { return _chapterModel; }
            private set { 
                _chapterModel = value;

                OnPropertyChanged(); 
            }
        }

        public async Task LoadImagesAsync()
        {
            if (_chapterModel != null)
            {
                if (_chapterModel.imagesFetched == false)
                {
                    var images = await _mangaService.GetChapterImagesOfChapter(_chapterModel.Id);

                    foreach (var image in images)
                    {
                        ChapterModel!.ChapterImageURLs.Add(image);
                    }

                    _chapterModel.imagesFetched = true;
                }
            }
        }

        public RelayCommand<ChapterModel> NextChapterCommand { get; }
        public RelayCommand<ChapterModel?> PreviousChapterCommand { get; }

        public ReadSceneVM(IChapterIteratorService chapterIterator, MangaService mangaService) 
        {
            _chapterIterator = chapterIterator;
            _chapterIterator.CurrentChapterChanged += OnChapterCreated!;

            _mangaService = mangaService;

            NextChapterCommand = new RelayCommand<ChapterModel>(_ => _chapterIterator.MoveToNextChapter());
            PreviousChapterCommand = new RelayCommand<ChapterModel?>(_ => _chapterIterator.MoveToPreviousChapter());
        }

        private void OnChapterCreated(object sender, EventArgs e)
        {
            ChapterModel = _chapterIterator.CurrentChapter;
        }
    }
}
