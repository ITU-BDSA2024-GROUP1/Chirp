using Chirp.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Data
{
    public class ChirpDBContext : DbContext
    {
        public DbSet<Cheep> Cheeps { get; set; }
        public DbSet<Author> Authors { get; set; }

        public ChirpDBContext(DbContextOptions<ChirpDBContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
