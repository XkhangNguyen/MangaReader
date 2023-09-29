using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Windows.Input;

namespace MangaReader.ViewModel
{
    public class ReadSceneVM : ViewModelBase
    {
        private MangaService _mangaService;

        private IChapterIteratorService _chapterIterator;
        public IChapterIteratorService ChapterIterator
        {
            get { return _chapterIterator; }
        }

        private Chapter? _chapter;
        public Chapter? Chapter
        {
            get { return _chapter; }
            private set { _chapter = value; OnPropertyChanged(); }
        }

        public RelayCommand<Chapter> NextChapterCommand { get; }
        public RelayCommand<Chapter?> PreviousChapterCommand { get; }

        public ReadSceneVM(IChapterIteratorService chapterIterator, MangaService mangaService) 
        {
            _chapterIterator = chapterIterator;
            _chapterIterator.CurrentChapterChanged += OnChapterCreated;

            _mangaService = mangaService;

            NextChapterCommand = new RelayCommand<Chapter>(_ => _chapterIterator.MoveToNextChapter());
            PreviousChapterCommand = new RelayCommand<Chapter>(_ => _chapterIterator.MoveToPreviousChapter());
        }

        private async void OnChapterCreated(object sender, EventArgs e)
        {
            Chapter = _chapterIterator.CurrentChapter;

            Chapter.ChapterImages = await _mangaService.GetChapterImagesOfChapter(Chapter);
        }
    }
}
