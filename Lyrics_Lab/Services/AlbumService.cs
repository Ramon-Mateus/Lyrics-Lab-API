using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories.Interfaces;
using Lyrics_Lab.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly ISongService _songService;

        public AlbumService(IAlbumRepository albumRepository, ISongService songService)
        {
            _albumRepository = albumRepository;
            _songService = songService;
        }

        public async Task<List<Album>> GetAlbums(int userId)
        {
            return await _albumRepository.GetAlbums(userId);
        }

        public async Task<Album?> GetAlbumById(int userId, int albumId)
        {
            return await _albumRepository.GetAlbumById(userId, albumId);
        }

        public async Task<Album> CreateAlbum(int userId, CreateAlbumDto createAlbumDto)
        {
            var album = new Album
            {
                Name = createAlbumDto.Name,
                Description = createAlbumDto.Description,
                Image = createAlbumDto.Image,
                UserId = userId
            };

            if (createAlbumDto.SongIds != null)
            {
                foreach (var songId in createAlbumDto.SongIds)
                {
                    var song = await _songService.GetSongById(userId, songId);
                    if (song != null) album.Songs.Add(song);
                }
            }

            return await _albumRepository.CreateAlbum(album);
        }

        public async Task<Album?> UpdateAlbum(int userId, int albumId, UpdateAlbumDto updateAlbumDto)
        {
            var album = await this.GetAlbumById(userId, albumId);

            if (album == null || album.IsDefault == true)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateAlbumDto.Name))
            {
                album.Name = updateAlbumDto.Name;
            }

            album.Description = !string.IsNullOrEmpty(updateAlbumDto.Description) ? updateAlbumDto.Description : null;
            album.Image = !string.IsNullOrEmpty(updateAlbumDto.Image) ? updateAlbumDto.Image : null;

            if (updateAlbumDto?.SongIds != null)
            {
                foreach (var song in album.Songs.ToList())
                {
                    if (!updateAlbumDto.SongIds.Contains(song.Id))
                    {
                        album.Songs.Remove(song);
                    }
                    else
                    {
                        updateAlbumDto.SongIds.Remove(song.Id);
                    }
                }

                foreach (var songId in updateAlbumDto.SongIds)
                {
                    var song = await _songService.GetSongById(userId, songId);
                    if (song != null) album.Songs.Add(song);
                }
            }

            return await _albumRepository.UpdateAlbum(album);
        }

        public async Task<bool> DeleteAlbum(int userId, int albumId)
        {
            var album = await this.GetAlbumById(userId, albumId);

            if (album == null || album.IsDefault == true)
            {
                return false;
            }

            return await _albumRepository.DeleteAlbum(album);
        }
    }
}
