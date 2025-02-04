using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;

namespace Lyrics_Lab.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAlbums(int userId);
        Task<Album?> GetAlbumById(int userId, int albumId);
        Task<Album> CreateAlbum(int userId, CreateAlbumDto createAlbumDto);
        Task<Album?> UpdateAlbum(int userId, int albumId, UpdateAlbumDto updateAlbumDto);
        Task<bool> DeleteAlbum(int userId, int albumId);
    }
}
