using System;
using System.Collections.Generic;

namespace MangaReader.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual List<Manga> Mangas { get; set; } = new List<Manga>();
}
