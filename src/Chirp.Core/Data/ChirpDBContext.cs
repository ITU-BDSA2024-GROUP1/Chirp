using Chirp.Core.Entities;
using System.Reflection.Emit;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Data;

public class ChirpDBContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cheep>()
                .HasOne(c => c.Author)
                .WithMany() // Assuming IdentityUser does not have a Cheeps navigation property
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

        //modelBuilder.Entity<Cheep>().Property(m => m.Text).IsRequired().HasMaxLength(160);
    }
}
