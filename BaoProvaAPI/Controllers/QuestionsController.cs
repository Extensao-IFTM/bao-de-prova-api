using BaoProvaAPI.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        [HttpGet("api-life")]
        public IActionResult Verify()
        {
            return Ok("API is running");
        }

        [HttpGet]
        public IActionResult GetQuestions()
        {
            string questionsJson = System.IO.File.ReadAllText("Data/questions.json");
            List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);
            
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            string questionsJson = System.IO.File.ReadAllText("Data/questions.json");
            List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(questionsJson);

            Question? question = questions?.FirstOrDefault(q => q.Id == id);

            if (question == null)
            {
                return NotFound($"Questão com o id {id} não foi encontrada");
            }

            return Ok(question);
        }
    }
}
