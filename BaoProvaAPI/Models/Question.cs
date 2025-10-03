namespace BaoProvaAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public required string Statement { get; set; }
        public string[] Alternatives { get; set; } = [];
        public int CorrectAlternative { get; set; }
        public string? Category { get; set; }
        public int Year { get; set; }
    }
}
