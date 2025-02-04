using Lyrics_Lab.Models;

namespace Lyrics_Lab.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        Task<List<Album>> GetAlbums(int userId);
        Task<Album?> GetAlbumById(int userId, int albumId);
        Task<Album> CreateAlbum(Album album);
        Task<Album> UpdateAlbum(Album album);
        Task<bool> DeleteAlbum(Album album);
    }
}
