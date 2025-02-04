using Lyrics_Lab.Models;

namespace Lyrics_Lab.Repositories.Interfaces
{
    public interface ISongRepository
    {
        Task<Song?> GetSongById(int userId, int songId);
    }
}
