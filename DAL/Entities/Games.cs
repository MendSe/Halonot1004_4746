using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Games
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }

        [JsonProperty("total_rating_count")]
        public int TotalRatingCount { get; set; }

        [JsonProperty("first_release_date")]
        public DateTime ReleaseDate { get; set; }
        
        [JsonProperty("cover")]
        public string CoverImageUrl { get; set; }

        [JsonProperty("genres")]
        public List<int> Genres { get; set; }

        [JsonProperty("platforms")]
        public List<int> Platforms { get; set; }

        [JsonProperty("websites")]
        public List<int> Websites { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public int UpdatedAt { get; set; }
    }

}
