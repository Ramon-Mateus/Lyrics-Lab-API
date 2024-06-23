using System.Text.Json.Serialization;

namespace Lyrics_Lab.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
