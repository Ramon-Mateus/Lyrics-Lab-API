using Lyrics_Lab.Contexts;
using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDbContext _context;

        public AlbumRepository(ApplicationDbContext applicationDbContext) => _context = applicationDbContext;

        public async Task<List<Album>> GetAlbums(int userId)
        {
            return await _context.Albums
                .Where(a => a.UserId == userId)
                .Include(a => a.Songs)
                .ToListAsync();
        }

        public async Task<Album?> GetAlbumById(int userId, int albumId)
        {
            return await _context.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == albumId && a.UserId == userId);
        }

        public async Task<Album?> GetAlbumById(int userId, bool isDefault)
        {
            return await _context.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault == isDefault);
        }

        public async Task<Album> CreateAlbum(Album album)
        {
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task<Album> UpdateAlbum(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task<bool> DeleteAlbum(Album album)
        {
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
