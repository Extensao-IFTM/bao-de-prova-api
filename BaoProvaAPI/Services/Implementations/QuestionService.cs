using BaoProvaAPI.Models;
using BaoProvaAPI.Services.Interfaces;
using System.Text.Json;

namespace BaoProvaAPI.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private const string QUESTIONSFILEPATH = "Data/questions.json";

        public List<Question> GetAllQuestions(string? category = null)
        {
            if (!System.IO.File.Exists(QUESTIONSFILEPATH))
            {   
                throw new FileNotFoundException("Arquivo de questões não encontrado");
            }

            string questionsJson = System.IO.File.ReadAllText(QUESTIONSFILEPATH);
            List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);

            if (questions == null)
            {
                throw new Exception("Erro ao carregar questões");
            }

            // Filtrar por categoria se fornecida
            if (!string.IsNullOrWhiteSpace(category))
            {
                questions = questions.Where(q => q.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }

            return questions;
        }

        public Question? GetQuestionById(int id)
        {
            if (!File.Exists(QUESTIONSFILEPATH))
            {
                throw new FileNotFoundException("Arquivo de questões não encontrado");
            }

            string questionsJson = File.ReadAllText(QUESTIONSFILEPATH);
            List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);

            return questions?.FirstOrDefault(q => q.Id == id);
        }

        public Question? GetRandomQuestion(string? category = null)
        {
            var questions = GetAllQuestions(category);

            if (!questions.Any())
            {
                return null;
            }

            Random random = new Random();
            return questions[random.Next(questions.Count)];
        }
    }
}
