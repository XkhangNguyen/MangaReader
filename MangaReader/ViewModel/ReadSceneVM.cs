using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Windows.Navigation;

namespace MangaReader.ViewModel
{
    public class ReadSceneVM : ViewModelBase
    {
        private IChapterIteratorService _chapterIterator;

        private ChapterModel? _chapterModel;
        int counter = 0;
        public ChapterModel? ChapterModel
        {
            get { return _chapterModel; }
            set 
            { 
                _chapterModel = value;
                OnPropertyChanged(); 
            }
        }

        public RelayCommand<ChapterModel> NextChapterCommand { get; }
        public RelayCommand<ChapterModel?> PreviousChapterCommand { get; }

        public ReadSceneVM(IChapterIteratorService chapterIterator) 
        {
            _chapterIterator = chapterIterator;
            _chapterIterator.CurrentChapterChanged += OnChapterCreated; // Subscribe to the event
            NextChapterCommand = new RelayCommand<ChapterModel>(_ => _chapterIterator.MoveToNextChapter());
            PreviousChapterCommand = new RelayCommand<ChapterModel>(_ => _chapterIterator.MoveToPreviousChapter());
        }

        private void OnChapterCreated(object sender, EventArgs e)
        {
            ChapterModel = _chapterIterator.CurrentChapter;
        }
    }
}
