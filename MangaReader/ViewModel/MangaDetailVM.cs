using MangaReader.Model;
using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Ultilities;
using System;
using System.Collections.ObjectModel;

namespace MangaReader.ViewModel
{
    internal class MangaDetailVM:Utilities.ViewModelBase
    {
        private MangaModel? _mangaModel;

        private readonly MangaStore _mangaStore;

        public RelayCommand<ChapterModel> ShowChapterDetailCommand { get; }

        private INavigationService? _navigation;

        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public MangaModel? MangaModel
        {
            get { return _mangaModel; }
            set { _mangaModel = value; OnPropertyChanged(); }
        }

        public MangaDetailVM(INavigationService navigationService, MangaStore mangaStore)
        {
            Navigation = navigationService;

            _mangaStore = mangaStore;

            _mangaStore.MangaCreated += OnMangaCreated;

            ShowChapterDetailCommand = new RelayCommand<ChapterModel>(ShowChapterDetail);
        }

        private void ShowChapterDetail(ChapterModel? chapterModel)
        {
            _mangaStore?.GetChapter(chapterModel);
            Navigation?.NavigateTo<ReadSceneVM>();
            }

        private void OnMangaCreated(MangaModel mangaModel)
        {
            MangaModel = mangaModel;
        }
    }
}
