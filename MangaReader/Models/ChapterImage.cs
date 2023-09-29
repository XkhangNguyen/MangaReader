using System;
using System.Collections.Generic;

namespace MangaReader.Models;

public partial class ChapterImage
{
    public int Id { get; set; }

    public string ChapterImageUrl { get; set; } = null!;

    public int? ChapterId { get; set; }

    public virtual Chapter? Chapter { get; set; }
}
