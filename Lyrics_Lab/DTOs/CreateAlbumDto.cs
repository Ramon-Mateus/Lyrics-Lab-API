using System.ComponentModel.DataAnnotations;

namespace Lyrics_Lab.DTOs
{
    public class CreateAlbumDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;
        public string? Image { get; set; }
        public List<int>? SongIds { get; set; } = new List<int>();
    }
}
