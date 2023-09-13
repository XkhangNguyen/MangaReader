﻿using MangaReader.Model;
using MangaReader.Services;
using MangaReader.ViewModel;
using System;
using System.Collections.Generic;


namespace MangaReader.Utilities
{
    public interface IChapterIteratorService
    {
        event EventHandler CurrentChapterChanged;
        ChapterModel CurrentChapter { get; }
        public void GetListChapters(List<ChapterModel> chapters);
        public void SetCurrentChapterIndex(int index);
        public bool MoveToNextChapter();
        public bool MoveToPreviousChapter();
    }
    public class ChapterIteratorService: IChapterIteratorService
    {
        private List<ChapterModel>? _chapters;
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

        public void GetListChapters(List<ChapterModel> chapters)
        {
            _chapters = chapters;
            _currentIndex = 0;
        }

        public ChapterModel CurrentChapter => _chapters[_currentIndex];

        public void SetCurrentChapterIndex(int index)
        {
            _currentIndex = index;
            CurrentChapterChanged?.Invoke(this, EventArgs.Empty);
            Navigation?.NavigateTo<ReadSceneVM>();
        }

        public bool MoveToNextChapter()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
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
                CurrentChapterChanged?.Invoke(this, EventArgs.Empty); // Trigger the event
                return true;
            }
            return false;
            
        }
    }

}