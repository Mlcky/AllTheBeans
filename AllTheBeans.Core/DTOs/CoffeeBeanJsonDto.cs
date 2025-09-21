using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AllTheBeans.Core.DTOs
{
    public class CoffeeBeanJsonDto
    {
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("isBOTD")]
        public bool IsBotd { get; set; }

        [JsonPropertyName("Cost")]
        public string? Cost { get; set; }

        [JsonPropertyName("Image")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("colour")]
        public string? Colour { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("Country")]
        public string? Country { get; set; }
    }
}
