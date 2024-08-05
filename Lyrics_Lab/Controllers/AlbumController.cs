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
    public class AlbumController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlbumController(ApplicationDbContext applicationDbContext) => _context = applicationDbContext;

        [HttpGet]
        public IActionResult GetAllAlbums()
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var albums = _context.Albums
                .Where(p => p.UserId == int.Parse(userId))
                .Include(p => p.Songs)
                .ToList();

            return Ok(albums);
        }

        [HttpGet("{Id}")]
        public IActionResult GetPlaylistById(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = _context.Albums
                .Include(p => p.Songs)
                .FirstOrDefault(p => p.Id == id && p.UserId == int.Parse(userId));

            if (album == null)
            {
                return NotFound(id);
            }

            return Ok(album);
        }

        [HttpPost]
        public async Task<ActionResult<Album>> CreatePlaylist([FromBody] CreatePlaylistDto createPlaylistDto)
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

            var playlist = new Album
            {
                Name = createPlaylistDto.Name,
                Description = createPlaylistDto.Description,
                UserId = int.Parse(userId)
            };

            _context.Albums.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] UpdateAlbumDto updateAlbumDto) {
            if  (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = await _context.Albums.FirstOrDefaultAsync(p => p.Id == id && p.UserId == int.Parse(userId));

            if (album == null)
            {
                return NotFound();
            }

            album.Name = updateAlbumDto.Name;
            album.Description = updateAlbumDto.Description;

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

            var playlist = await _context.Albums.FirstOrDefaultAsync(p => p.Id == id && p.UserId == int.Parse(userId));

            if (playlist == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
