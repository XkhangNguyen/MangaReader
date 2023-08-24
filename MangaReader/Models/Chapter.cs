using System.Collections.Generic;

namespace MangaReader.Models
{
    public class Chapter
    {
        public float ChapterNumber { get; set; }
        public string ChapterLink { get; set; } = string.Empty;
        public List<string> ChapterImageURLs { get; set; } = new List<string>();
    }
}
