using System.Text.Json.Serialization;

namespace Lyrics_Lab.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Lyric { get; set; } = null;
        public string? Tone { get; set; } = null;
        public string? Compass { get; set; } = null;
        public float? Bpm { get; set; } = null;
        public bool? Sustenido { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
