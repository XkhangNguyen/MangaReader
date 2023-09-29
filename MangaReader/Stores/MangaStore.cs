using MangaReader.Model;
using System;
using System.Collections.ObjectModel;
using MangaReader.Models;


namespace MangaReader.Stores
{
    public class MangaStore
    {
        public event Action<Manga>? MangaCreated;
        public void GetManga(Manga? mangaModel)
        {
            if (mangaModel != null)
                MangaCreated?.Invoke(mangaModel);
        }

        private ObservableCollection<Manga>? _mangasList;

        public event Action<ObservableCollection<Manga>>? MangasListCreated
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

        private event Action<ObservableCollection<Manga>>? OnMangasListCreated;

        public void GetMangas(ObservableCollection<Manga> MangasList)
        {
            _mangasList = MangasList;            
        }
    }
}
