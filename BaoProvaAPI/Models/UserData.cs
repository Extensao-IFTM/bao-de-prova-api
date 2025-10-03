namespace BaoProvaAPI.Models
{
    public class UserData
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAlternative { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
