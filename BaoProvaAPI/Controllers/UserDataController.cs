using BaoProvaAPI.Models;
using BaoProvaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _userDataService;

        public UserDataController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        [HttpPost]
        public IActionResult SaveUserData([FromBody] UserData userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userDataService.SaveUserAnswer(userData);
                return Ok(new { message = "Resposta salva com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao salvar resposta: {ex.Message}" });
            }
        }

        [HttpGet("user/{userId}/stats")]
        public IActionResult GetUserStats(int userId)
        {
            try
            {
                var stats = _userDataService.GetUserStats(userId);

                if (stats.TotalQuestions == 0)
                {
                    return NotFound(new { message = $"Nenhum dado encontrado para o usuário {userId}" });
                }

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao obter estatísticas: {ex.Message}" });
            }
        }

        [HttpGet("user/{userId}/history")]
        public IActionResult GetUserHistory(int userId)
        {
            try
            {
                var history = _userDataService.GetUserHistory(userId);

                if (!history.Any())
                {
                    return NotFound(new { message = $"Nenhum histórico encontrado para o usuário {userId}" });
                }

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao obter histórico: {ex.Message}" });
            }
        }

        [HttpGet("ranking")]
        public IActionResult GetRanking()
        {
            try
            {
                var ranking = _userDataService.GetRanking();
                return Ok(ranking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao obter ranking: {ex.Message}" });
            }
        }
    }
}
