using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MangaReader.Model
{
    public class MangaModel
    {
        public int Id { get; set; }
        public string MangaTitle { get; set; } = string.Empty;
        public string MangaDescription { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public List<GenreModel>? Genres { get; set; }
        public List<ChapterModel>? Chapters { get; set; }
    }
}
