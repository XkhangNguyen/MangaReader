using MangaReader.Model;
using System;
using System.Collections.ObjectModel;

namespace MangaReader.Stores
{
    public class MangaStore
    {
        private ObservableCollection<MangaModel>? _mangasList;

        public event Action<MangaModel>? MangaCreated;
        public void GetManga(MangaModel? mangaModel)
        {
            if (mangaModel != null)
                MangaCreated?.Invoke(mangaModel);
        }

        private event Action<ObservableCollection<MangaModel>>? OnMangasListCreated;

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

        public void FetchMangasData(ObservableCollection<MangaModel> MangasList)
        {
            _mangasList = MangasList;            
        }
    }
}
