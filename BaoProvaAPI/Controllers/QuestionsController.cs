using BaoProvaAPI.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using BaoProvaAPI.Services.Interfaces;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

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
                List<Question> questions = _questionService.GetAllQuestions(category);
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
                var question = _questionService.GetQuestionById(id);

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
                var question = _questionService.GetRandomQuestion(category);

                if (question == null)
                {
                    return NotFound("Nenhuma questão disponível");
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar questão: {ex.Message}");
            }
        }
    }
}
