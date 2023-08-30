using MangaReader.Model;
using System.Collections.ObjectModel;
using MangaReader.Stores;
using MangaReader.Ultilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MangaReader.Services;

namespace MangaReader.ViewModel
{
    internal class MangasDisplayVM : Utilities.ViewModelBase
    {
        private readonly MangaStore _mangaStore;
        public RelayCommand<MangaModel> ShowMangaDetailCommand { get; }

        private INavigationService? _navigation;

        public INavigationService? Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MangaModel>? _mangaModels;
        public ObservableCollection<MangaModel>? MangaModels
        {
            get { return _mangaModels; }
            set
            {
                _mangaModels = value;
                OnPropertyChanged();
            }
        }

        public MangasDisplayVM(INavigationService navigationService, MangaStore mangaStore)
        {
            Navigation = navigationService;

            _mangaStore = mangaStore;

            _mangaStore.MangasListCreated += OnMangasListCreated;

            ShowMangaDetailCommand = new RelayCommand<MangaModel>(ShowMangaDetail);

        }

        private void OnMangasListCreated(ObservableCollection<MangaModel> mangaModels)
        {
           MangaModels = mangaModels;
        }

        public override void Dispose()
        {
            _mangaStore.MangasListCreated -= OnMangasListCreated;

            base.Dispose();
        }

        private void ShowMangaDetail(MangaModel? manga)
        {
            _mangaStore.GetManga(manga);

            Navigation?.NavigateTo<MangaDetailVM>();
        }


    }
}
