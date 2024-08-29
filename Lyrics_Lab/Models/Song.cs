using System.Text.Json.Serialization;

namespace Lyrics_Lab.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Lyric { get; set; } = null;
        public string? Tone { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
