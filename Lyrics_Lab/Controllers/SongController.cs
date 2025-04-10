using Lyrics_Lab.Contexts;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Lyrics_Lab.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lyrics_Lab.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISongService _songService;

        public SongController(ApplicationDbContext context, ISongService songService)
        {
            _context = context;
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var songs = await _songService.GetSongs(int.Parse(userId));

            return Ok(songs);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSongById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var song = await _songService.GetSongById(int.Parse(userId), id);

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var song = await _songService.CreateSong(int.Parse(userId), createSongDto);

            if(song == null)
            {
                return NotFound(new { message = "Album default não cadastrado no sistema." });
            }

            return CreatedAtAction(nameof(GetSongById), new { id = song.Id}, song);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] UpdateSongDto updateSongDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var result = await _songService.UpdateSong(int.Parse(userId), id, updateSongDto);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }
            
            var result = await _songService.DeleteSong(int.Parse(userId), id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
