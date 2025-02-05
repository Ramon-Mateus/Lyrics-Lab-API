using Lyrics_Lab.Models;

namespace Lyrics_Lab.Repositories.Interfaces
{
    public interface ISongRepository
    {
        Task<List<Song>> GetSongs(int userId);
        Task<Song?> GetSongById(int userId, int songId);
        Task<Song> CreateSong(Song song, Album defaultAlbum);
        Task RemoveSongFromAlbums(int userId, int songId);
        Task UpdateSong();
        Task<bool> DeleteSong(Song song);
    }
}
