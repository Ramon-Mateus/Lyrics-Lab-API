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
        public int AlbumId { get; set; }
    }
}
