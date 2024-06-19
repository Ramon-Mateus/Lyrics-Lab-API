using Lyrics_Lab.Contexts;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lyrics_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllSongs()
        {
            var songs = _context.Songs.ToList();
            return Ok(songs);
        }

        [HttpGet("{Id}")]
        public IActionResult GetSongById(int Id)
        {
            var song = _context.Songs.FirstOrDefault(x => x.Id == Id);

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
        public async Task<IActionResult> UpdateSong(int Id, [FromBody] UpdateSongDto updateSongDto)
        {
            if (updateSongDto == null)
            {
                return BadRequest("Nenhum dado para atualizar");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var song = await _context.Songs.FindAsync(Id);
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
        public async Task<IActionResult> DeleteSong(int Id)
        {
            var song = await _context.Songs.FindAsync(Id);

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
