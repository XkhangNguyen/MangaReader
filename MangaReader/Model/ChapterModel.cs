using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MangaReader.Model
{
    public class ChapterModel
    {
        public int Id { get; set; }
        public MangaModel? MangaModel { get; set; }
        public string ChapterNumber { get; set; } = string.Empty;
        public string ChapterLink { get; set; } = string.Empty;
        public ObservableCollection<string> ChapterImageURLs { get; set; } = new ObservableCollection<string>();

        public bool imagesFetched = false;
    }
}
