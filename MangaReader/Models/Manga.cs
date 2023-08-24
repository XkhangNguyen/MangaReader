using System.Collections.Generic;

namespace MangaReader.Models
{
    public class Manga
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageURL { get; set; } = string.Empty;
        public List<string> Genre { get; set; } = new List<string>();
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
        public string MangaLink { get; set; } = string.Empty;
    }
}
