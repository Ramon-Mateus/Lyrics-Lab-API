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
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistController(ApplicationDbContext applicationDbContext) => _context = applicationDbContext;

        [HttpGet]
        public IActionResult GetAllPLaylists()
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var playlists = _context.Playlists.Where(p => p.UserId == int.Parse(userId)).ToList();
            return Ok(playlists);
        }

        [HttpGet("{Id}")]
        public IActionResult GetPlaylistById(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var playlist = _context.Playlists.FirstOrDefault(p => p.Id == id && p.UserId == int.Parse(userId));

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

            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado."});
            }

            var playlist = new Playlist
            {
                Name = createPlaylistDto.Name,
                Description = createPlaylistDto.Description,
                UserId = int.Parse(userId)
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] UpdatePlaylistDto updatePlaylistDto) {
            if  (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.Id == id && p.UserId == int.Parse(userId));

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
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var playlist = await _context.Playlists.FirstOrDefaultAsync(p => p.Id == id && p.UserId == int.Parse(userId));

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
