using System.ComponentModel.DataAnnotations;

namespace Lyrics_Lab.DTOs
{
    public class UpdateAlbumDto
    {
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;
        public string? Image { get; set; }
    }
}
