using MangaReader.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MangaReader.Services
{
    public class MangaService
    {
        private readonly MangaDbContext _mangaDbContext;

        public MangaService(MangaDbContext mangaDbContext)
        {
            _mangaDbContext = mangaDbContext;
        }

        public async Task<ObservableCollection<Manga>> GetAllMangas()
        {
            // Use EF Core to retrieve all mangas from the database.
            var mangas = new ObservableCollection<Manga>(
                        _mangaDbContext.Mangas
                        .Include(m => m.Chapters)
                        .Include(m => m.Genres)
                        .ToList());
            return mangas;
        }

        public async Task<Manga> GetMangaOfChapter(Chapter chapter)
        {
            // Find the manga that contains the specified chapter.
            var mangaContainingChapter = await _mangaDbContext.Mangas
                .Where(m => m.Chapters.Any(c => c.Id == chapter.Id))
                .FirstOrDefaultAsync();

            return mangaContainingChapter;
        }

        public async Task<List<ChapterImage>> GetChapterImagesOfChapter(Chapter chapter)
        {
            return await _mangaDbContext.ChapterImages
                .Where(ci => ci.ChapterId == chapter.Id)
                .ToListAsync();
        }


        public async Task<List<Manga>> GetAllMangasOfGenre(Genre genre)
        {
            // Retrieve all mangas that have the specified genre.
            var mangasWithGenre = await _mangaDbContext.Mangas
                .Where(m => m.Genres.Any(g => g.Id == genre.Id))
                .ToListAsync();

            return mangasWithGenre;
        }
    }
}
