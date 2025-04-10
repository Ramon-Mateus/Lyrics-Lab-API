using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Song> Songs => Set<Song>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });

            modelBuilder.Entity<Album>()
                .HasOne(u => u.User)
                .WithMany(a => a.Albums)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Song>()
                .HasMany(a => a.Albums)
                .WithMany(s => s.Songs)
                .UsingEntity(sa => sa.ToTable("SongAlbum"));
        }
    }
}
