using BaoProvaAPI.Models;

namespace BaoProvaAPI.Services.Interfaces
{
    public interface IUserDataService
    {
        void SaveUserAnswer(UserData userData);
        List<UserData> GetUserHistory(int userId);
        UserStats GetUserStats(int userId);
        List<RankingEntry> GetRanking();
    }

    public class UserStats
    {
        public int UserId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int Score { get; set; }
        public double Accuracy { get; set; }
    }

    public class RankingEntry
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
    }
}
