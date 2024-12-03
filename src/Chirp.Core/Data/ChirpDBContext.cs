using Chirp.Core.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Data;

public class ChirpDBContext(DbContextOptions<ChirpDBContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Follow> Follows { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cheep>()
            .HasOne(c => c.Author)
            .WithMany() // Assuming IdentityUser does not have a Cheeps navigation property
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Follow>()
            .HasOne(c => c.Follower)
            .WithMany()
            .HasForeignKey(c => c.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Follow>()
            .HasOne(c => c.Followed)
            .WithMany()
            .HasForeignKey(c => c.FollowedId)
            .OnDelete(DeleteBehavior.Cascade);
        //modelBuilder.Entity<Cheep>().Property(m => m.Text).IsRequired().HasMaxLength(160);
    }
}