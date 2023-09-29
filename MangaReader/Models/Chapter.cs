using System;
using System.Collections.Generic;

namespace MangaReader.Models;

public partial class Chapter
{
    public int Id { get; set; }

    public string ChapterNumber { get; set; } = null!;

    public string ChapterLink { get; set; } = null!;

    public int? MangaId { get; set; }

    public virtual List<ChapterImage> ChapterImages { get; set; } = new List<ChapterImage>();

    public virtual Manga? Manga { get; set; }
}
