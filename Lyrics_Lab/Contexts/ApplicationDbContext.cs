using Lyrics_Lab.Models;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Playlist> Playlists => Set<Playlist>();
        public DbSet<Song> Songs => Set<Song>();
    }
}
