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

        public async Task<List<Song>> GetSongs(int userId)
        {
            return await _context.Songs.Where(s => s.Albums.Any(a => a.UserId == userId))
                .Include(s => s.Albums)
                .ToListAsync();
        }

        public async Task<Song?> GetSongById(int userId, int songId)
        {
            return await _context.Songs
                .Where(s => s.Id == songId && s.Albums.Any(a => a.UserId == userId))
                .Include(s => s.Albums)
                .FirstOrDefaultAsync();
        }

        public async Task<Song> CreateSong(Song song, Album defaultAlbum)
        {
            defaultAlbum.Songs.Add(song);
            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
            return song;
        }

        public async Task RemoveSongFromAlbums(int userId, int songId)
        {
            await _context.Database.ExecuteSqlRawAsync(@"
            DELETE FROM SongAlbum
            WHERE SongsId = {0} AND AlbumsId IN (
                SELECT Id FROM Albums WHERE UserId = {1} AND IsDefault != true
            )", songId, userId);
        }

        public async Task UpdateSong()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteSong(Song song)
        {
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
