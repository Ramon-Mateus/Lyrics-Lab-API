using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;

namespace Lyrics_Lab.Services.Interfaces
{
    public interface ISongService
    {
        Task<List<Song>> GetSongs(int userId);
        Task<Song?> GetSongById(int userId, int songId);
        Task<Song?> CreateSong(int userId, CreateSongDto createSongDto);
        Task<bool> UpdateSong(int userId, int songId, UpdateSongDto updateSongDto);
        Task<bool> DeleteSong(int userId, int songId);
    }
}
