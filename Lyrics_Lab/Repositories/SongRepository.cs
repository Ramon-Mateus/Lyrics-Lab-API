using Lyrics_Lab.Contexts;
using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly ApplicationDbContext _context;

        public SongRepository(ApplicationDbContext applicationDbContext) => _context = applicationDbContext;

        public async Task<Song?> GetSongById(int userId, int songId)
        {
            return await _context.Songs
                .Where(s => s.Id == songId && s.Albums.Any(a => a.UserId == userId))
                .Include(s => s.Albums)
                .FirstOrDefaultAsync();
        }
    }
}
