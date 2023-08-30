using System.Collections.Generic;

namespace MangaReader.Model
{
    public class ChapterModel
    {
        public MangaModel? MangaModel { get; set; }
        public float ChapterNumber { get; set; }
        public string ChapterLink { get; set; } = string.Empty;
        public List<string>? ChapterImageURLs { get; set; }
    }
}
