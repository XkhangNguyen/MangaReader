using MangaReader.Data;
using MangaReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task AddMangaAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
        }

    }
}
