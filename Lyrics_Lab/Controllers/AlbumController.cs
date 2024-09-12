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
                .Where(a => a.UserId == int.Parse(userId))
                .Include(a =>a.Songs)
                .ToList();

            return Ok(albums);
        }

        [HttpGet("{Id}")]
        public IActionResult GetAlbumById(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = _context.Albums
                .Include(a => a.Songs)
                .FirstOrDefault(a => a.Id == id && a.UserId == int.Parse(userId));

            if (album == null)
            {
                return NotFound(id);
            }

            return Ok(album);
        }

        [HttpPost]
        public async Task<ActionResult<Album>> CreateAlbum([FromBody] CreateAlbumDto createAlbumDto)
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

            var album = new Album
            {
                Name = createAlbumDto.Name,
                Description = createAlbumDto.Description,
                Image = createAlbumDto.Image,
                UserId = int.Parse(userId)
            };

            if (createAlbumDto.SongIds != null) {
                foreach (var songId in createAlbumDto.SongIds)
                {
                    var song = await _context.Songs.FirstOrDefaultAsync(s =>
                        s.Id == songId && s.Albums.Any(a => a.UserId == int.Parse(userId)));
                    if (song != null) album.Songs.Add(song);
                }
            }

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbumById), new { id = album.Id }, album);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] UpdateAlbumDto updateAlbumDto) {
            if  (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = await _context.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == int.Parse(userId));

            if (album == null || album.IsDefault == true)
            {
                return NotFound();
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
                    var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == songId && s.Albums.Any(a => a.UserId == int.Parse(userId)));
                    if(song != null) album.Songs.Add(song);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var userId = User.FindFirstValue("iss");

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == id && a.UserId == int.Parse(userId));

            if (album == null || album.IsDefault == true)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPut("user/{Id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
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
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }
            
            if (!string.IsNullOrEmpty(updateUserDto.Name))
            {
                user.Name = updateUserDto.Name;
            }
            
            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email;
            }
            
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            }
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
