using Lyrics_Lab.Models;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
