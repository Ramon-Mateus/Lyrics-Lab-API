using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Lyrics_Lab.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
