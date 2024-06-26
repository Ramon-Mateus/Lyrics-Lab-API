﻿using System.Text.Json.Serialization;

namespace Lyrics_Lab.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Lyric { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int PlaylistId { get; set; }
        [JsonIgnore]
        public Playlist? Playlist { get; set; }
    }
}
