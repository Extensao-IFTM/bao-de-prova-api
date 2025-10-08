using BaoProvaAPI.Models;
using BaoProvaAPI.Services.Interfaces;
using System.Text.Json;

namespace BaoProvaAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private const string USERSFILEPATH = "Data/users.json";

        public User? GetUserById(int id)
        {
            List<User> users = LoadUsers();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByEmail(string email)
        {
            List<User> users = LoadUsers();
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User CreateUser(User user)
        {
            List<User> users = LoadUsers();

            // Define um novo ID
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            user.CreatedAt = DateTime.Now;

            users.Add(user);
            SaveUsers(users);

            return user;
        }

        public bool UserExistsByEmail(string email)
        {
            return GetUserByEmail(email) != null;
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(USERSFILEPATH))
                return new List<User>();

            string json = File.ReadAllText(USERSFILEPATH);

            if (string.IsNullOrWhiteSpace(json))
                return new List<User>();

            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

            // Cria o diretório se não existir
            string? directory = Path.GetDirectoryName(USERSFILEPATH);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(USERSFILEPATH, json);
        }

        public List<User> GetAllUsers()
        {
            return LoadUsers();
        }
    }
}
