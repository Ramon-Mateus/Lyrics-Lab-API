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

            var songs = _context.Songs.Where(s => s.Playlist.UserId == int.Parse(userId)).ToList();
            
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

            var song = _context.Songs.FirstOrDefault(s => s.Id == id && s.Playlist.UserId == int.Parse(userId));

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

            var playlist = await _context.Playlists.FindAsync(createSongDto.PlaylistId);

            if (playlist == null || playlist.UserId != int.Parse(userId))
            {
                return Forbid();
            }

            var song = new Song
            {
                Name = createSongDto.Name,
                Lyric = createSongDto.Lyric,
                PlaylistId = createSongDto.PlaylistId
            };

            _context.Songs.Add(song);
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

            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id && s.Playlist.UserId == int.Parse(userId));

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

            if (updateSongDto.PlaylistId.HasValue)
            {
                song.PlaylistId = updateSongDto.PlaylistId.Value;
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

            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id && s.Playlist.UserId == int.Parse(userId));

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
