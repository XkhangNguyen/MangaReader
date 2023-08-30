using System.Collections.Generic;

namespace MangaReader.Model
{
    public class MangaModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageURL { get; set; } = string.Empty;
        public List<string> Genre { get; set; } = new List<string>();
        public List<ChapterModel> Chapters { get; set; } = new List<ChapterModel>();
        public string MangaLink { get; set; } = string.Empty;
    }
}
