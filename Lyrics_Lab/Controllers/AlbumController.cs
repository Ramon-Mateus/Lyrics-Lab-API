using Lyrics_Lab.Contexts;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories.Interfaces;
using Lyrics_Lab.Services.Interfaces;
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
        private readonly IAlbumService _albumService;

        public AlbumController(ApplicationDbContext applicationDbContext, IAlbumService albumService)
        {
            _context = applicationDbContext;
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAlbums()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var albums = await _albumService.GetAlbums(int.Parse(userId));

            return Ok(albums);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAlbumById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = await _albumService.GetAlbumById(int.Parse(userId), id);

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado."});
            }

            var album = await _albumService.CreateAlbum(int.Parse(userId), createAlbumDto);

            return CreatedAtAction(nameof(GetAlbumById), new { id = album.Id }, album);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] UpdateAlbumDto updateAlbumDto) {
            if  (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var album = await _albumService.UpdateAlbum(int.Parse(userId), id, updateAlbumDto);

            if(album == null)
            {
                return NotFound(new { messsage = "Album não encontrado" });
            }

            return Ok(album);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(new { message = "Usuário não autenticado." });
            }

            var result = await _albumService.DeleteAlbum(int.Parse(userId), id);

            if(!result)
            {
                return NotFound(new { message = "Album não encontrado" });
            }

            return NoContent();
        }
    }
}
