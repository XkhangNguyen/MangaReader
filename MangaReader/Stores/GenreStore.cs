using MangaReader.Model;
using System;
using System.Collections.ObjectModel;


namespace MangaReader.Stores
{
    public class GenreStore
    {
        private ObservableCollection<GenreModel>? _genresList;

        private event Action<ObservableCollection<GenreModel>>? OnGenresListCreated;

        public event Action<ObservableCollection<GenreModel>>? GenresListCreated
        {
            add
            {
                OnGenresListCreated += value;

                if (_genresList != null)
                {
                    value?.Invoke(_genresList);
                }
            }
            remove
            {
                OnGenresListCreated -= value;
            }
        }

        public void FetchGenresData(ObservableCollection<GenreModel> GenresList)
        {
            _genresList = GenresList;
        }
    }
}
