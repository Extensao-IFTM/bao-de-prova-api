using System.Text.Json.Serialization;

namespace BaoProvaAPI.Models
{
    public class Question
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("statement")]
        public required string Statement { get; set; }

        [JsonPropertyName("alternatives")]
        public string[] Alternatives { get; set; } = [];

        [JsonPropertyName("correctAlternative")]
        public int CorrectAlternative { get; set; }

        [JsonPropertyName("explanation")]
        public string? Explanation {get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }
    }
}
