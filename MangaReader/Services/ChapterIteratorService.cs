using MangaReader.Models;
using MangaReader.Services;
using MangaReader.ViewModel;
using System;
using System.Collections.Generic;


namespace MangaReader.Utilities
{
    public interface IChapterIteratorService
    {
        event EventHandler CurrentChapterChanged;
        Chapter CurrentChapter { get; }
        Manga CurrentManga { get; }
        public void GetListChapters(List<Chapter> chapters);
        public void SetCurrentChapterIndex(int index, Manga mangaRef);
        public bool MoveToNextChapter();
        public bool MoveToPreviousChapter();
    }
    public class ChapterIteratorService: IChapterIteratorService
    {
        private List<Chapter>? _chapters;

        private Manga currentManga;

        private int _currentIndex;
        
        public event EventHandler CurrentChapterChanged;

        private INavigationService? _navigation;
        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value;}
        }
        public ChapterIteratorService(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public void GetListChapters(List<Chapter> chapters)
        {
            _chapters = chapters;
            _currentIndex = 0;
        }

        public Chapter CurrentChapter => _chapters[_currentIndex];
        public Manga CurrentManga => currentManga;

        public void SetCurrentChapterIndex(int index, Manga mangaRef)
        {
            _currentIndex = index;
            currentManga = mangaRef;
            _chapters[_currentIndex].Manga = currentManga;
            CurrentChapterChanged?.Invoke(this, EventArgs.Empty);
            Navigation?.NavigateTo<ReadSceneVM>();
        }

        public bool MoveToNextChapter()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                _chapters[_currentIndex].Manga = currentManga;
                CurrentChapterChanged?.Invoke(this, EventArgs.Empty);
                Navigation?.NavigateTo<ReadSceneVM>();
                return true;
            }
            return false;
        }

        public bool MoveToPreviousChapter()
        {
            if (_currentIndex < _chapters.Count - 1)
            {
                _currentIndex++;
                _chapters[_currentIndex].Manga = currentManga;
                CurrentChapterChanged?.Invoke(this, EventArgs.Empty); // Trigger the event
                return true;
            }
            return false;
            
        }
    }

}
