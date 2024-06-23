using System.Text.Json.Serialization;

namespace Lyrics_Lab.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
