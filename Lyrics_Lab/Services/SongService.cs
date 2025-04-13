using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories.Interfaces;
using Lyrics_Lab.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumService _albumService;

        public SongService(ISongRepository songRepository, IAlbumService albumService)
        {
            _songRepository = songRepository;
            _albumService = albumService;
        }

        public async Task<List<Song>> GetSongs(int userId)
        {
            return await _songRepository.GetSongs(userId);
        }

        public async Task<Song?> GetSongById(int userId, int songId)
        {
            return await _songRepository.GetSongById(userId, songId);
        }

        public async Task<Song?> CreateSong(int userId, CreateSongDto createSongDto)
        {
            var song = new Song
            {
                Name = createSongDto.Name,
                Lyric = createSongDto.Lyric,
                Tone = createSongDto.Tone,
                Compass = createSongDto.Compass,
                Bpm = createSongDto.Bpm,
                Sustenido = createSongDto.Sustenido
            };

            if (createSongDto.AlbumIds != null && createSongDto.AlbumIds.Count > 0)
            {
                foreach (var albumId in createSongDto.AlbumIds.Distinct())
                {
                    var album = await _albumService.GetAlbumById(userId, albumId);
                    album?.Songs.Add(song);
                }
            }

            var defaultAlbum = await _albumService.GetAlbumById(userId, true);

            if (defaultAlbum == null)
            {
                return null;
            }

            return await _songRepository.CreateSong(song, defaultAlbum!);
        }

        public async Task<bool> UpdateSong(int userId, int songId, UpdateSongDto updateSongDto)
        {
            var song = await this.GetSongById(userId, songId);

            if (song == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(updateSongDto.Name))
            {
                song.Name = updateSongDto.Name;
            }

            song.Lyric = !string.IsNullOrEmpty(updateSongDto.Lyric) ? updateSongDto.Lyric : null;
            song.Tone = !string.IsNullOrEmpty(updateSongDto.Tone) ? updateSongDto.Tone : null;
            song.Compass = !string.IsNullOrEmpty(updateSongDto.Compass) ? updateSongDto.Compass : null;
            song.Bpm = updateSongDto.Bpm;
            song.Sustenido = updateSongDto.Sustenido;

            if (updateSongDto?.AlbumIds != null && updateSongDto?.AlbumIds?.Count != 0)
            {
                var albums = await _albumService.GetAlbums(userId);

                if (albums.Count > 0)
                {
                    await _songRepository.RemoveSongFromAlbums(userId, songId);
                }

                foreach (var albumId in updateSongDto.AlbumIds.Distinct())
                {
                    var album = albums.FirstOrDefault(a => a.Id == albumId && a.UserId == userId);
                    album?.Songs.Add(song);
                }
            }

            await _songRepository.UpdateSong();
            return true;
        }

        public async Task<bool> DeleteSong(int userId, int songId)
        {
            var song = await this.GetSongById(userId, songId);
            if (song == null)
            {
                return false;
            }
            return await _songRepository.DeleteSong(song);
        }
    }
}
