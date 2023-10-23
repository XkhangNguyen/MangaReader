using System.Collections.ObjectModel;


namespace MangaReader.Model
{
    public class MangaModel
    {
        public int Id { get; set; }
        public string MangaTitle { get; set; } = string.Empty;
        public string MangaDescription { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public ObservableCollection<GenreModel> Genres { get; set; } = new ObservableCollection<GenreModel>();
        public ObservableCollection<ChapterModel> Chapters { get; set; } = new ObservableCollection<ChapterModel>();

        public bool chaptersFetched = false;
    }
}
