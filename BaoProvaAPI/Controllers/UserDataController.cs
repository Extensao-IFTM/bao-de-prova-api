using BaoProvaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BaoProvaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private const string USERDATAFILEPATH = "Data/userdata.json";
        private const string USERSFILEPATH = "Data/users.json";

        [HttpPost]
        public IActionResult SaveUserData([FromBody] UserData userData)
        {
            List<UserData> userDataList = LoadUserData();

            // Adiciona timestamp se não fornecido
            if (userData.Timestamp == default)
            {
                userData.Timestamp = DateTime.Now;
            }

            userDataList.Add(userData);
            SaveUserDataToFile(userDataList);

            return Ok(new { message = "Dados do usuário salvos com sucesso." });
        }

        private List<UserData> LoadUserData()
        {
            if (!System.IO.File.Exists(USERDATAFILEPATH))
            {
                return new List<UserData>();
            }

            string json = System.IO.File.ReadAllText(USERDATAFILEPATH);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<UserData>();
            }

            return JsonSerializer.Deserialize<List<UserData>>(json) ?? new List<UserData>();
        }

        private void SaveUserDataToFile(List<UserData> userDataList)
        {
            string json = JsonSerializer.Serialize(userDataList, new JsonSerializerOptions { WriteIndented = true });

            // Cria o diretório se não existir
            string? directory = Path.GetDirectoryName(USERDATAFILEPATH);
            
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            System.IO.File.WriteAllText(USERDATAFILEPATH, json);
        }
    }
}
