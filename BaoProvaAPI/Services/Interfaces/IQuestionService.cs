using BaoProvaAPI.Models;

namespace BaoProvaAPI.Services.Interfaces
{
    public interface IQuestionService
    {
        List<Question> GetAllQuestions(string? category = null);
        Question? GetQuestionById(int id);
        Question? GetRandomQuestion(string? category = null);
    }
}
