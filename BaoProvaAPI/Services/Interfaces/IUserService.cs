using BaoProvaAPI.Models;

namespace BaoProvaAPI.Services.Interfaces
{
    public interface IUserService
    {
        User? GetUserById(int id);
        User? GetUserByEmail(string email);
        User CreateUser(User user);
        bool UserExistsByEmail(string email);
        List<User> GetAllUsers();
    }
}
