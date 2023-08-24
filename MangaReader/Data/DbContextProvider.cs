using Microsoft.EntityFrameworkCore;

namespace MangaReader.Data
{
    public class DbContextProvider
    {
        private readonly string _connectionString;

        public DbContextProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ApplicationDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
