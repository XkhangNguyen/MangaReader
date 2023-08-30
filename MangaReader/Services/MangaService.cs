using MangaReader.Data;
using MangaReader.Model;

using System.Threading.Tasks;

namespace MangaReader.Services
{
    public class MangaService
    {
        private readonly ApplicationDbContext _context;

        public MangaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMangaAsync(MangaModel manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
        }

    }
}
