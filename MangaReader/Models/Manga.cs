using System;
using System.Collections.Generic;

namespace MangaReader.Models;

public partial class Manga
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? Author { get; set; }

    public virtual List<Chapter> Chapters { get; set; } = new List<Chapter>();

    public virtual List<Genre> Genres { get; set; } = new List<Genre>();
}
