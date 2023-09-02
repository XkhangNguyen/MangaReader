using System.Collections.Generic;

namespace MangaReader.Model
{
    public class ChapterModel
    {
        public MangaModel? MangaModel { get; set; }
        public string ChapterNumber { get; set; } = string.Empty;
        public string ChapterLink { get; set; } = string.Empty;
        public List<string>? ChapterImageURLs { get; set; }
    }
}
