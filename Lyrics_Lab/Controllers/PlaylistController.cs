using Lyrics_Lab.Contexts;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistController(ApplicationDbContext applicationDbContext) => _context = applicationDbContext;

        [HttpGet]
        public IActionResult GetAllPLaylists()
        {
            var playlists = _context.Playlists.ToList();
            return Ok(playlists);
        }

        [HttpGet("{Id}")]
        public IActionResult GetPlaylistById(int id)
        {
            var playlist = _context.Playlists.FirstOrDefault(x => x.Id == id);

            if (playlist == null)
            {
                return NotFound(id);
            }

            return Ok(playlist);
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> CreatePlaylist([FromBody] CreatePlaylistDto createPlaylistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = new Playlist
            {
                Name = createPlaylistDto.Name,
                Description = createPlaylistDto.Description
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePlaylist(int Id, [FromBody] UpdatePlaylistDto updatePlaylistDto) {
            if  (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _context.Playlists.FirstOrDefaultAsync(x => x.Id == Id);

            if (playlist == null)
            {
                return NotFound();
            }

            playlist.Name = updatePlaylistDto.Name;
            playlist.Description = updatePlaylistDto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async  Task<IActionResult> DeletePlaylist(int Id) 
        {
            var playlist = await _context.Playlists.FindAsync(Id);

            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
