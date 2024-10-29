using Chirp.Core.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Data
{
    public class ChirpDBContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Cheep> Cheeps { get; set; }
        public DbSet<Author> Authors { get; set; }

        public ChirpDBContext(DbContextOptions<ChirpDBContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
