using BaoProvaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        [HttpPost]
        public IActionResult SaveUserData([FromBody] UserData userData)
        {
            var filePath = "Data/userdata.json";
            List<UserData> userDataList = new();

            if (System.IO.File.Exists(filePath))
            {
                var existingJson = System.IO.File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    userDataList = JsonSerializer.Deserialize<List<UserData>>(existingJson) ?? new List<UserData>();
                }
            }

            userDataList.Add(userData);
            var updatedJson = JsonSerializer.Serialize(userDataList, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, updatedJson);

            return Ok(new { message = "Dados do usuário salvos com sucesso." });
        }
    }
}
