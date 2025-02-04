using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories.Interfaces;
using Lyrics_Lab.Services.Interfaces;

namespace Lyrics_Lab.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<Song?> GetSongById(int userId, int songId)
        {
            return await _songRepository.GetSongById(userId, songId);
        }
    }
}
