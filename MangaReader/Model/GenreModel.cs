using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Model
{
    public class GenreModel
    {
        public int Id { get; set; }
        [JsonProperty("genre_name")]
        public string GenreName { get; set; } = string.Empty;
    }
}
