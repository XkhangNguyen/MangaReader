using MangaReader.Model;
using System;
using System.Collections.ObjectModel;

namespace MangaReader.Stores
{
    public class MangaStore
    {
        public event Action<MangaModel>? MangaCreated;
        public void GetManga(MangaModel? mangaModel)
        {
            if (mangaModel != null)
                MangaCreated?.Invoke(mangaModel);
        }

        private ObservableCollection<MangaModel>? _mangasList;

        public event Action<ObservableCollection<MangaModel>>? MangasListCreated
        {
            add
            {
                OnMangasListCreated += value;

                if (_mangasList != null)
                {
                    value?.Invoke(_mangasList);
                }
            }
            remove
            {
                OnMangasListCreated -= value;
            }
        }

        private event Action<ObservableCollection<MangaModel>>? OnMangasListCreated;

        public void FetchMangasData(ObservableCollection<MangaModel> MangasList)
        {
            _mangasList = MangasList;            
        }
    }
}
