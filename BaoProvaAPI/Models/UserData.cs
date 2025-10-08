using System.ComponentModel.DataAnnotations;

namespace BaoProvaAPI.Models
{
    public class UserData
    {
        [Required(ErrorMessage = "UserId é obrigatório")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "QuestionId é obrigatório")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "SelectedAlternative é obrigatório")]
        [Range(0, 4, ErrorMessage = "SelectedAlternative deve estar entre 0 e 4")]
        public int SelectedAlternative { get; set; }

        public bool IsCorrect { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
