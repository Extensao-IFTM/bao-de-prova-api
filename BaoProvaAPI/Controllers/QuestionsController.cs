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
    }
}
