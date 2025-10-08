using BaoProvaAPI.Models;
using BaoProvaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_userService.UserExistsByEmail(user.Email))
                {
                    return BadRequest(new { message = "Já existe um usuário com este email." });
                }

                var createdUser = _userService.CreateUser(user);
                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = createdUser.Id },
                    createdUser
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao criar usuário: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);

                if (user == null)
                {
                    return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao buscar usuário: {ex.Message}" });
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                var user = _userService.GetUserByEmail(email);

                if (user == null)
                {
                    return NotFound(new { message = $"Usuário com email {email} não encontrado" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao buscar usuário: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                // Você precisará adicionar este método no IUserService
                var users = _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao buscar usuários: {ex.Message}" });
            }
        }
    }
}
