using BaoProvaAPI.Models;
using BaoProvaAPI.Services.Interfaces;
using System.Text.Json;

namespace BaoProvaAPI.Services.Implementations
{
    public class UserDataService : IUserDataService
    {
        private const string USERDATAFILEPATH = "Data/userdata.json";
        private readonly IUserService _userService;

        public UserDataService(IUserService userService)
        {
            _userService = userService;
        }

        public void SaveUserAnswer(UserData userData)
        {
            // Validar se usuário existe
            User? user = _userService.GetUserById(userData.UserId);
            if (user == null)
            {
                throw new Exception($"Usuário com ID {userData.UserId} não existe.");
            }

            List<UserData> userDataList = LoadUserData();

            if (userData.Timestamp == default)
            {
                userData.Timestamp = DateTime.Now;
            }

            userDataList.Add(userData);
            SaveUserData(userDataList);
        }

        public List<UserData> GetUserHistory(int userId)
        {
            List<UserData> allUserData = LoadUserData();
            return allUserData
                .Where(ud => ud.UserId == userId)
                .OrderByDescending(ud => ud.Timestamp)
                .ToList();
        }

        public UserStats GetUserStats(int userId)
        {
            List<UserData> userData = LoadUserData()
                .Where(ud => ud.UserId == userId)
                .ToList();

            int totalQuestions = userData.Count;
            int correctAnswers = userData.Count(ud => ud.IsCorrect);
            int wrongAnswers = totalQuestions - correctAnswers;

            return new UserStats
            {
                UserId = userId,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                Score = CalculateScore(userData),
                Accuracy = totalQuestions > 0 ? (correctAnswers * 100.0 / totalQuestions) : 0
            };
        }

        public List<RankingEntry> GetRanking()
        {
            List<UserData> allUserData = LoadUserData();

            return allUserData
                .GroupBy(ud => ud.UserId)
                .Select(g =>
                {
                    var user = _userService.GetUserById(g.Key);
                    var userDataList = g.ToList();

                    return new RankingEntry
                    {
                        UserId = g.Key,
                        UserName = user?.Name ?? "Usuário Desconhecido",
                        Score = CalculateScore(userDataList),
                        TotalQuestions = userDataList.Count,
                        CorrectAnswers = userDataList.Count(ud => ud.IsCorrect)
                    };
                })
                .OrderByDescending(r => r.Score)
                .ToList();
        }

        private List<UserData> LoadUserData()
        {
            if (!File.Exists(USERDATAFILEPATH))
            {
                return new List<UserData>();
            }

            string json = File.ReadAllText(USERDATAFILEPATH);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<UserData>();
            }

            return JsonSerializer.Deserialize<List<UserData>>(json) ?? new List<UserData>();
        }

        private void SaveUserData(List<UserData> userDataList)
        {
            string json = JsonSerializer.Serialize(userDataList, new JsonSerializerOptions { WriteIndented = true });

            string? directory = Path.GetDirectoryName(USERDATAFILEPATH);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(USERDATAFILEPATH, json);
        }

        private static int CalculateScore(List<UserData> userData)
        {
            return userData.Sum(ud => ud.IsCorrect ? 10 : -2);
        }
    }
}
