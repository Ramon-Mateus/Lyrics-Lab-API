using Lyrics_Lab.Contexts;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetPLaylistById(int id) {
            var playlist = _context.Playlists.FirstOrDefault(x => x.Id == id);
            return Ok(playlist);
        }
    }
}
