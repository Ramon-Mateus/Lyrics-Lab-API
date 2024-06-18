using Lyrics_Lab.Contexts;
using Microsoft.AspNetCore.Http;
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
    }
}
