using Lyrics_Lab.Models;

namespace Lyrics_Lab.Services.Interfaces
{
    public interface ISongService
    {
        Task<Song?> GetSongById(int userId, int songId);
    }
}
