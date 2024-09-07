using Lyrics_Lab.Contexts;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Lyrics_Lab.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllSongs()
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var songs = _context.Songs.Where(s => s.Albums.Any(a => a.UserId == int.Parse(userId)))
                .Include(s => s.Albums)
                .ToList();
            
            return Ok(songs);
        }

        [HttpGet("{Id}")]
        public IActionResult GetSongById(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }
            
            var song = _context.Songs.Where(s => s.Id == id && s.Albums.Any(a => a.UserId == int.Parse(userId)))
                .Include(s => s.Albums)
                .FirstOrDefault();

            if (song == null)
            {
                return NotFound();
            }
            
            return Ok(song);
        }

        [HttpPost]
        public async Task<ActionResult<Song>> CreateSong([FromBody] CreateSongDto createSongDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var song = new Song
            {
                Name = createSongDto.Name,
                Lyric = createSongDto.Lyric,
                Tone = createSongDto.Tone,
                Compass = createSongDto.Compass,
                Bpm = createSongDto.Bpm
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            
            var defaultAlbum = await _context.Albums.FirstOrDefaultAsync(a => a.UserId == int.Parse(userId) && a.IsDefault == true);

            if (defaultAlbum == null)
            {
                return NotFound("Default album not found.");
            }
            
            defaultAlbum.Songs.Add(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSongById), new { id = song.Id}, song);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] UpdateSongDto updateSongDto)
        {
            if (updateSongDto == null)
            {
                return BadRequest("Nenhum dado para atualizar");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id && s.Albums.Any(a => a.UserId == int.Parse(userId)));
            
            if (song == null)
            {
                return NotFound();
            }
            
            if (!string.IsNullOrEmpty(updateSongDto.Name))
            {
                song.Name = updateSongDto.Name;
            }
            
            if (!string.IsNullOrEmpty(updateSongDto.Lyric))
            {
                song.Lyric = updateSongDto.Lyric;
            }
            
            if (!string.IsNullOrEmpty(updateSongDto.Tone))
            {
                song.Tone = updateSongDto.Tone;
            }
            
            if (!string.IsNullOrEmpty(updateSongDto.Compass))
            {
                song.Compass = updateSongDto.Compass;
            }

            if (updateSongDto.Bpm.HasValue)
            {
                song.Bpm = updateSongDto.Bpm;
            }

            if (updateSongDto?.AlbumIds != null)
            {
                var albums = await _context.Albums
                    .Where(a => a.UserId == int.Parse(userId) && a.IsDefault != true)
                    .ToListAsync();

                if (albums.Count > 0)
                {
                    await _context.Database.ExecuteSqlRawAsync(@"
                       DELETE FROM SongAlbum
                       WHERE SongsId = {0} AND AlbumsId IN (
                           SELECT Id
                           FROM Albums
                           WHERE UserId = {1} AND IsDefault != true
                       )
                       ", song.Id, userId);
                }
                
                foreach (var albumId in updateSongDto.AlbumIds.Distinct())
                {
                    var album = albums.FirstOrDefault(a => a.Id == albumId);
                    album?.Songs.Add(song);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id && s.Albums.Any(a => a.UserId == int.Parse(userId)));
            
            if (song == null)
            {
                return NotFound();
            }
            
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
