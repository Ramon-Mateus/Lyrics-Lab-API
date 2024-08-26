using System.ComponentModel.DataAnnotations;

namespace Lyrics_Lab.DTOs
{
    public class UpdateSongDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? Lyric { get; set; }
        public string? Tone { get; set; }
        public int? AlbumId { get; set; }
    }
}
