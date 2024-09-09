using System.ComponentModel.DataAnnotations;

namespace Lyrics_Lab.DTOs
{
    public class CreateSongDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Lyric { get; set; }
        public string? Tone { get; set; }
        public float? Bpm { get; set; }
        public string? Compass { get; set; }
        public List<int>? AlbumIds { get; set; } = new List<int>();
    }
}
