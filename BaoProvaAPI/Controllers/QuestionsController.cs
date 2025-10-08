using BaoProvaAPI.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private const string QUESTIONSFILEPATH = "Data/questions.json";

        [HttpGet("api-life")]
        public IActionResult Verify()
        {
           return Ok(new { status = "API is running", timestamp = DateTime.Now });
        }

        [HttpGet]
        public IActionResult GetQuestions([FromQuery] string? category = null)
        {
            try
            {
                if (!System.IO.File.Exists(QUESTIONSFILEPATH))
                {
                    return NotFound("Arquivo de questões não encontrado");
                }

                string questionsJson = System.IO.File.ReadAllText(QUESTIONSFILEPATH);
                List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);

                if (questions == null)
                {
                    return StatusCode(500, "Erro ao carregar questões");
                }

                // Filtrar por categoria se fornecida
                if (!string.IsNullOrWhiteSpace(category))
                {
                    questions = questions.Where(q => q.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true).ToList();
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar questões: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            try
            {
                if (!System.IO.File.Exists(QUESTIONSFILEPATH))
                {
                    return NotFound("Arquivo de questões não encontrado");
                }

                string questionsJson = System.IO.File.ReadAllText(QUESTIONSFILEPATH);
                List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);

                Question? question = questions?.FirstOrDefault(q => q.Id == id);

                if (question == null)
                {
                    return NotFound($"Questão com o id {id} não foi encontrada");
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar questão: {ex.Message}");
            }
        }

        [HttpGet("random")]
        public IActionResult GetRandomQuestion([FromQuery] string? category = null)
        {
            try
            {
                if (!System.IO.File.Exists(QUESTIONSFILEPATH))
                {
                    return NotFound("Arquivo de questões não encontrado");
                }

                string questionsJson = System.IO.File.ReadAllText(QUESTIONSFILEPATH);
                List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);

                if (questions == null || !questions.Any())
                {
                    return NotFound("Nenhuma questão disponível");
                }

                // Filtrar por categoria se fornecida
                if (!string.IsNullOrWhiteSpace(category))
                {
                    questions = questions.Where(q => q.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true).ToList();
                }

                if (!questions.Any())
                {
                    return NotFound($"Nenhuma questão encontrada para a categoria {category}");
                }

                Random random = new Random();
                Question randomQuestion = questions[random.Next(questions.Count)];

                return Ok(randomQuestion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar questão: {ex.Message}");
            }
        }
    }
}
