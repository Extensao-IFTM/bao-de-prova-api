using System.ComponentModel.DataAnnotations;

namespace BaoProvaAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
